using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Game.Logic.Actions
{
    public class CheckPVEGameStateAction : IAction
    {
        private long m_time;
        private bool m_isFinished;

        public CheckPVEGameStateAction(int delay)
        {
            m_time = TickHelper.GetTickCount() + delay;
            m_isFinished = false;
        }

        public void Execute(BaseGame game, long tick)
        {
            if (m_time > tick || game.GetWaitTimer() >= tick)
            {
                return;
            }

            PVEGame pve = game as PVEGame;
            if (pve == null)
            {
                m_isFinished = true;
                return;
            }

            switch (pve.GameState)
            {
                // ─────────────────────────────────────────
                // Oyun ilk oluşturuldu, hazırlık başlasın
                // ─────────────────────────────────────────
                case eGameState.Inited:
                    pve.Prepare();
                    break;

                // ─────────────────────────────────────────
                // Hazırlık tamam, yeni etabı hazırla
                // ─────────────────────────────────────────
                case eGameState.Prepared:
                    pve.PrepareNewSession();
                    break;

                // ─────────────────────────────────────────
                // Yükleme ekranı: tüm oyuncular hazır mı?
                // ─────────────────────────────────────────
                case eGameState.Loading:
                    if (pve.IsAllComplete())
                        pve.StartGame();
                    else
                        game.WaitTime(1000);
                    break;

                // ─────────────────────────────────────────
                // Oyun sahnesi kuruldu, canlıları hazırla
                // ─────────────────────────────────────────
                case eGameState.GameStart:
                    if (game.RoomType == eRoomType.FightLab)
                    {
                        if (game.CurrentActionCount <= 1)
                            pve.PrepareFightingLivings();
                    }
                    else
                    {
                        pve.PrepareNewGame();
                    }
                    break;

                // ─────────────────────────────────────────
                // Oyun oynuyor: sıradaki hamle veya bitiş
                // ─────────────────────────────────────────
                case eGameState.Playing:
                    // Saldırı animasyonu veya başka aksiyon sürüyorsa bekle
                    if ((pve.CurrentLiving != null && pve.CurrentLiving.IsAttacking)
                        || game.CurrentActionCount > 1)
                    {
                        break;
                    }

                    if (pve.CanGameOver())
                    {
                        // Labirent modunda kapıya giriş animasyonu varsa önce onu oynat
                        if (pve.IsLabyrinth() && pve.CanEnterGate)
                        {
                            pve.GameOverMovie();
                        }
                        else if (pve.CurrentActionCount <= 1)
                        {
                            if (pve.IsCanPrepareGameOver())
                                pve.PrepareGameOver();
                            else
                                pve.GameOver();
                        }
                    }
                    else
                    {
                        if (pve.GameStateModify == eGameState.Waiting)
                            pve.WaitingGameState();
                        else
                            pve.NextTurn();
                    }
                    break;

                // ─────────────────────────────────────────
                // Bekleme modu (özel event/animasyon vs.)
                // ─────────────────────────────────────────
                case eGameState.Waiting:
                    pve.WaitingGameState();
                    break;

                // ─────────────────────────────────────────
                // Oyun sonu öncesi hazırlık (skor ekranı vs.)
                // ─────────────────────────────────────────
                case eGameState.PrepareGameOver:
                    if (pve.CanEndGame)
                        pve.GameOver();
                    else
                        pve.PrepareGameOver();
                    break;

                // ─────────────────────────────────────────────────────────
                // Etap bitti → sonraki etap var mı? Kazandı mı? Kaybetti mi?
                // ─────────────────────────────────────────────────────────
                case eGameState.GameOver:
                    if (pve.IsWin)
                    {
                        if (pve.HasNextSession())
                        {
                            // Kazandı ve devam eden etap var → sonraki etaba geç
                            pve.PrepareNewSession();
                        }
                        else
                        {
                            // Kazandı ve tüm etaplar bitti → oyunu tamamen bitir
                            pve.GameOverAllSession();
                            game.WaitTime(23000);
                        }
                    }
                    else
                    {
                        // Kaybetti → tüm oturumu durdur (yeniden deneme ALLSessionStopped'da)
                        pve.GameOverAllSession();
                        game.WaitTime(5000);
                    }
                    break;

                // ─────────────────────────────────────────
                // Etap hazır: stil ve yükleme başlasın
                // ─────────────────────────────────────────
                case eGameState.SessionPrepared:
                    if (pve.CanStartNewSession())
                    {
                        pve.SetupStyle();
                        pve.StartLoading();
                    }
                    else
                    {
                        game.WaitTime(1000);
                    }
                    break;

                // ──────────────────────────────────────────────────────────────
                // Tüm oturum durdu: oyuncu sayısı, kazanma, yeniden deneme kontrolü
                // ──────────────────────────────────────────────────────────────
                case eGameState.ALLSessionStopped:

                    // Odada kimse kalmadıysa direkt durdur
                    if (pve.PlayerCount == 0)
                    {
                        pve.Stop();
                        break;
                    }

                    // Oyuncu yeniden denemek istemiyor → durdur
                    if (pve.WantTryAgain == 0)
                    {
                        pve.Stop();
                        break;
                    }

                    // Kazandı, tüm etaplar bitti, büyük kart ekranı gösterilecekse bekle
                    if (pve.IsWin && !pve.HasNextSession())
                    {
                        if (pve.IsShowLargeCards())
                            game.WaitTime(23000);
                        else
                            game.WaitTime(23000);
                        break;
                    }

                    // Kaybetti, sonraki etap da yok → kısa bekleme sonrası durdur
                    if (!pve.IsWin && !pve.HasNextSession())
                    {
                        game.WaitTime(5000);
                        break;
                    }

                    // Yeniden deneme: aynı etabı tekrar oyna (1 veya 2 fark etmez)
                    if (pve.WantTryAgain == 1 || pve.WantTryAgain == 2)
                    {
                        pve.SessionId--;
                        pve.PrepareNewSession();
                        break;
                    }

                    // Hiçbir koşula uymadıysa kısa bekle ve tekrar kontrol et
                    game.WaitTime(1000);
                    break;
            }

            m_isFinished = true;
        }

        public bool IsFinished(long tick)
        {
            return m_isFinished;
        }
    }
}