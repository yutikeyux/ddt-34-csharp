<?php
include('global.php');

if(isset($_POST['login']) && !isset($_SESSION['UserData']))
{
    if(!isset($_POST['UserName']))
    {
        echo Alert("Kullanıcı adının doldurulması gerekmektedir");
    }
    else if(!isset($_POST['Password']))
    {
        echo Alert("Şifre Alanını Doldurmanız Zorunludur");
    }
    else
    {
        $user = addslashes($_POST['UserName']);
        $pass = $_POST['Password'];
        $UserInfo = new MemberData($user,$pass);
        if($UserInfo->isAuthenticated())
        {
        getUserData($UserInfo);
        $_SESSION['UserData'] = serialize($UserInfo);
        }
        else
        {
            echo Alert("Şifre veya Giriş Bilgileri Yanlış");
        }

        
    }
}


if(isset($_POST["register"]))
{


    @$username = addslashes($_POST['Username']);
    @$password = $_POST['password'];
    @$Repassword = $_POST['Repassword'];
    @$Nickname = $_POST['Nickname'];
    @$sex = (int)$_POST['sex'];
    @$email = $_POST['Email'];
    $text_r = '';
    if($username == null || $password == null || $Repassword == null || $Nickname == null || $email == null)
    {
        $text_r .= "Tüm alanların doldurulması zorunludur";
    }
    if(!preg_match("/^([a-zA-Z0-9\-\_]*)$/",$username) || !preg_match("/^([a-zA-Z0-9\-\_]*)$/",$Nickname)) {
        $text_r .= 'Geçersiz giriş veya takma ad';
    }
    if(!filter_var($email,FILTER_VALIDATE_EMAIL)) $text_r .= ' e-postanız geçerli değil <br>';
    if($password != $Repassword) $text_r.= 'Şifreleriniz aynı değil <br>';
    if(strlen($username)  < 6 || strlen($username)  > 30) $text_r .= 'Kullanıcı adı 6 ila 30 karakter uzunluğunda olmalıdır <br>';
    if(strlen($password)  < 6 || strlen($password)  > 30) $text_r .= 'Şifre 6 ile 30 karakter arasında olmalıdır <br>';
    if(strlen($Nickname)  < 6 || strlen($Nickname)  > 30) $text_r .= 'Takma ad 6 ila 30 karakter uzunluğunda olmalıdır <br>';
    if (strpos("1".$Nickname,"GM") or strpos("1".$Nickname,"à¸ˆà¸µà¹€à¸­à¹‡à¸¡") or strpos("1".$Nickname,"Gunny") or strpos("1".$Nickname,"Game Master")or strpos("1".strtolower($Nickname),"adm")or strpos("1".strtolower($Nickname),"gm")or strpos("1".strtolower($Nickname),"mod")) {
        $text_r .="ADM, MOD, GM KELİMELERİ TAKMA ADINIZDA KULLANILAMAZ";
    }
    if($text_r == '') {
        co();
        $password = strtoupper(md5($password));
        $q = q("Select TOP 1 UserId From Mem_Users Where UserName = '{$username}'");
        if(qn($q) == 0) {
            $q = q("Select TOP 1 UserId From Webshop_Account Where Email = '{$email}'");
            if(qn($q) == 0) {
                $q = q("Select TOP 1 UserId From ".$dbtank41.".dbo.Sys_Users_Detail Where NickName = '{$Nickname}'");
                if(qn($q) == 0) {
                    q("exec ".$config['Database'].".dbo.Webshop_Register @ApplicationName=N'DanDanTang',@UserName=N'{$username}',@password=N'{$password}',@email='{$email}',@passtwo = '".strtoupper(md5($password))."',@error = 0");
                    q("exec ".$dbtank41.".dbo.SP_Users_Active @UserID='',@Attack=0,@Colors=N',,,,,,',@ConsortiaID=0,@Defence=0,@Gold=100000,@GP=0,@Grade=1,@Luck=0,@Money=0,@Style=N',,,,,,',@Agility=0,@State=0,@UserName=N'{$username}',@PassWord=N'{$password}',@Sex='".$sex."',@Hide=1111111111,@ActiveIP=N'',@Skin=N'',@Site=N''");
                    if($sex == 1) {
                        q("exec ".$dbtank41.".dbo.SP_Users_RegisterNotValidate @UserName=N'".$username."',@PassWord=N'{$password}',@NickName=N'{$Nickname}',@BArmID=7001,@BHairID=3101,@BFaceID=6102,@BClothID=5101,@BHatID=1101,@GArmID=7001,@GHairID=3101,@GFaceID=6102,@GClothID=5101,@GHatID=1101,@ArmColor=N'',@HairColor=N'',@FaceColor=N'',@ClothColor=N'',@HatColor=N'',@Sex='{$sex}',@StyleDate=0");
                    }
                    else {
                        q ("exec ".$dbtank41.".dbo.SP_Users_RegisterNotValidate @UserName=N'{$username}',@PassWord=N'{$password}',@NickName=N'{$Nickname}',@BArmID=7001,@BHairID=3201,@BFaceID=6225,@BClothID=5201,@BHatID=1201,@GArmID=7001,@GHairID=3201,@GFaceID=6202,@GClothID=5201,@GHatID=1201,@ArmColor=N'',@HairColor=N'',@FaceColor=N'',@ClothColor=N'',@HatColor=N'',@Sex='{$sex}',@StyleDate=0");
                    }
                    q("exec ".$dbtank41.".dbo.SP_Users_LoginWeb @UserName=N'{$username}',@Password=N'',@FirstValidate=0,@NickName=N'{$Nickname}'");
                    
                    echo Alert("Kayıt tamamlandı, lütfen giriş yapın");
                } else echo Alert("Bu takma ad zaten kullanılıyor");
            } else  echo Alert("Bu e-posta zaten kullanılıyor");
        } else echo Alert("Bu giriş zaten kullanılıyor");
    }
    else
    {
        echo Alert($text_r);
    }
    

}

if(isset($_POST['logout']) || isset($_GET['logout']))
{
    unset($_SESSION["UserData"]);
    header('Location: .');
}

?>
<script type="text/javascript">
function minuscula(z){
v = z.value.toLowerCase();
z.value = v;
}
</script>
<!DOCTYPE html>
<html lang="tr-TR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title><?php echo $titulo; ?></title>
    <meta name="Description" content="<?php echo $description; ?>">
    <meta name="Keywords" content="<?php echo $KeyWords; ?>">
    <link rel="shortcut icon" href="./Assets/images/favicon.ico">
    <link rel="stylesheet" href="./Assets/bootstrap/3.3.7/css/dbootstrap.min.css">
    <link rel="stylesheet" href="./Assets/css/tatli-stil.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Orbitron:wght@400;700;900&family=Rajdhani:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
/* İlkbahar Temalı Yeniden Tasarım */
body {
  padding-top: 80px;
  font-family: 'Rajdhani', sans-serif;
  background-color: #f8fafc; /* Çok açık mavi-beyaz */
  color: #1e293b; /* Koyu gri-mavi */
  overflow-x: hidden;
}

h1, h2, h3, h4, h5, h6 {
  font-family: 'Orbitron', sans-serif;
  font-weight: 700;
  color: #166534; /* Orman yeşili */
}

/* Sabit Navigasyon */
.fixed-nav {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  background: rgba(255, 255, 255, 0.92);
  backdrop-filter: blur(8px);
  z-index: 1000;
  padding: 15px 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  border-bottom: 1px solid #e2e8f0;
}

.nav-logo {
  color: #166534;
  font-size: 1.8rem;
  font-weight: 900;
  margin-left: 30px;
  text-decoration: none;
  font-family: 'Orbitron', sans-serif;
  background: linear-gradient(45deg, #16a34a, #ca8a04);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  text-shadow: 0 0 8px rgba(22, 163, 74, 0.3);
  animation: glowSpring 2s infinite alternate;
}

@keyframes glowSpring {
  from { text-shadow: 0 0 8px rgba(22, 163, 74, 0.3); }
  to { text-shadow: 0 0 16px rgba(251, 191, 36, 0.6), 0 0 24px rgba(22, 163, 74, 0.4); }
}

.fixed-nav ul {
  display: flex;
  list-style: none;
  margin-right: 30px;
}

.fixed-nav ul li {
  margin: 0 15px;
}

.fixed-nav ul li a {
  color: #1e293b;
  text-decoration: none;
  font-size: 1.1rem;
  font-weight: 600;
  transition: all 0.3s ease;
  position: relative;
  padding: 5px 0;
}

.fixed-nav ul li a::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(45deg, #16a34a, #ca8a04);
  transition: width 0.3s ease;
}

.fixed-nav ul li a:hover::after {
  width: 100%;
}

.fixed-nav ul li a:hover {
  color: #ca8a04;
  text-shadow: 0 0 8px rgba(251, 191, 36, 0.5);
}

/* Ana Giriş Bölümü */
.hero-section {
  height: 100vh;
  background: linear-gradient(rgba(255, 255, 255, 0.7), rgba(240, 249, 232, 0.9)), url('https://images.unsplash.com/photo-1501854140801-50d01698950b?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
  border-bottom: 1px solid #cbd5e1;
}

.hero-bg {
  display: none; /* Artık gerekmiyor */
}

.hero-content {
  position: relative;
  z-index: 1;
  display: flex;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.hero-left {
  flex: 1;
  padding-right: 40px;
}

.hero-right {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
}

.hero-title {
  font-size: 4rem;
  font-weight: 900;
  margin-bottom: 20px;
  background: linear-gradient(45deg, #16a34a, #ca8a04, #fbbf24);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  text-shadow: 0 0 10px rgba(251, 191, 36, 0.4);
  animation: textGlowSpring 3s infinite alternate;
}

@keyframes textGlowSpring {
  from { text-shadow: 0 0 8px rgba(251, 191, 36, 0.4); }
  to { text-shadow: 0 0 16px rgba(251, 191, 36, 0.7), 0 0 24px rgba(22, 163, 74, 0.5); }
}

.hero-subtitle {
  font-size: 1.8rem;
  margin-bottom: 30px;
  color: #1e293b;
  font-weight: 500;
}

/* Giriş Formu */
.login-card {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 25px;
  padding: 40px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 450px;
  margin: 0 auto;
  border: 1px solid #e2e8f0;
  animation: slideIn 1s ease-out;
  color: #1e293b;
}

.login-header h2 {
  font-size: 2.5rem;
  margin-bottom: 10px;
  background: linear-gradient(45deg, #16a34a, #ca8a04);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.login-header p {
  color: #64748b;
  font-size: 1.1rem;
}

.form-control {
  border: 1px solid #cbd5e1;
  border-radius: 15px;
  padding: 15px 20px;
  font-size: 1.1rem;
  background: white;
  color: #1e293b;
  transition: all 0.3s ease;
}

.form-control:focus {
  box-shadow: 0 0 0 3px rgba(22, 163, 74, 0.2);
  border-color: #16a34a;
}

.btn-login {
  background: linear-gradient(45deg, #16a34a, #ca8a04);
  color: white;
  border: none;
  border-radius: 50px;
  padding: 15px;
  font-size: 1.2rem;
  font-weight: 700;
  width: 100%;
  margin-top: 10px;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(22, 163, 74, 0.3);
}

.btn-login:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 16px rgba(22, 163, 74, 0.5);
}

.login-footer a {
  color: #ca8a04;
  font-weight: 700;
  text-decoration: none;
  transition: color 0.3s ease;
}

.login-footer a:hover {
  color: #fbbf24;
  text-shadow: 0 0 8px rgba(251, 191, 36, 0.5);
}

/* Tanıtım Listesi Stilleri */
.intro-container {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 25px;
  padding: 30px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 500px;
  height: 500px;
  position: relative;
  overflow: hidden;
  border: 1px solid #e2e8f0;
}

.intro-page {
  display: none;
  height: calc(100% - 60px); /* Pagination height'ı düş */
  overflow-y: auto;
  padding-right: 10px;
}

.intro-page.active {
  display: block;
  animation: fadeIn 0.5s ease-in-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}

.intro-page h3 {
  font-size: 1.8rem;
  margin-bottom: 15px;
  color: #166534;
  text-align: center;
}

.intro-page p {
  margin-bottom: 15px;
  line-height: 1.6;
}

.intro-page img {
  max-width: 100%;
  border-radius: 15px;
  margin: 15px 0;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.pagination {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 15px;
  padding: 0 10px;
}

.pagination button {
  background: linear-gradient(45deg, #16a34a, #ca8a04);
  color: white;
  border: none;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  justify-content: center;
  align-items: center;
}

.pagination button:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 8px rgba(22, 163, 74, 0.3);
}

.pagination button:disabled {
  background: #cbd5e1;
  cursor: not-allowed;
  transform: scale(1);
}

#pageIndicator {
  font-weight: bold;
  color: #166534;
  font-size: 1.1rem;
  font-family: 'Orbitron', sans-serif;
}

/* Genel Bölüm Stilleri */
.section {
  padding: 80px 0;
  position: relative;
}

.section-title h2 {
  font-size: 3rem;
  margin-bottom: 15px;
  background: linear-gradient(45deg, #16a34a, #ca8a04, #fbbf24);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  text-shadow: 0 0 10px rgba(251, 191, 36, 0.2);
  animation: pulseSpring 2s infinite alternate;
}

@keyframes pulseSpring {
  from { transform: scale(1); }
  to { transform: scale(1.02); }
}

.section-title p {
  font-size: 1.2rem;
  color: #64748b;
  max-width: 700px;
  margin: 0 auto;
}

/* Silahlar Kartı */
.weapon-card {
  background: white;
  border-radius: 20px;
  overflow: hidden;
  width: 300px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.08);
  transition: all 0.4s ease;
  border: 1px solid #e2e8f0;
  position: relative;
}

.weapon-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 5px;
  background: linear-gradient(45deg, #16a34a, #ca8a04);
}

.weapon-info h3 {
  color: #166534;
}

.weapon-info p {
  color: #475569;
}

.weapon-stat {
  background: rgba(22, 163, 74, 0.1);
  color: #16a34a;
  border: 1px solid rgba(22, 163, 74, 0.2);
}

/* Galeri */
.gallery-item {
  border-radius: 15px;
  overflow: hidden;
  height: 250px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.08);
}

.gallery-overlay {
  background: linear-gradient(to top, rgba(0, 0, 0, 0.7), transparent);
  color: white;
}

/* Wiki, Forum, Oyuncu Kartları */
.wiki-card,
.forum-card,
.player-card {
  background: white;
  border-radius: 20px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.08);
  border: 1px solid #e2e8f0;
}

.wiki-card {
  border-left: 5px solid #16a34a;
}

.player-stat-value {
  color: #ca8a04;
}

/* Responsive */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }
  .section-title h2 {
    font-size: 2.2rem;
  }
  .intro-container {
    height: 400px;
  }
}
</style>
</head>
<body>

    <!-- Sabit Navigasyon -->
    <nav class="fixed-nav">
        <a href="#giris" class="nav-logo">Tr Bombom</a>
        <ul>
            <li><a href="#giris">Giriş</a></li>
            <li><a href="#silahlar">Silahlar</a></li>
            <li><a href="#galeri">Galeri</a></li>
            <li><a href="#wiki">Wiki</a></li>
            <li><a href="#forum">Forum</a></li>
            <li><a href="#oyuncular">Oyuncular</a></li>
        </ul>
    </nav>

    <!-- Ana Giriş Bölümü -->
    <section id="giris" class="hero-section">
        <div class="hero-bg"></div>
        <div class="hero-content">
            <div class="hero-left">
                <h1 class="hero-title">Tr Bombom Evrenine Hoş Geldin!</h1>
                <p class="hero-subtitle">Efsanevi maceralara atıl, arkadaşlarınla savaş ve en güçlü bombomcu sen ol!</p>
                
                <?php if(!isset($_SESSION['UserData'])): ?>
                <div class="login-card">
                    <div class="login-header">
                        <h2>Giriş Yap</h2>
                        <p>Hesabınıza giriş yapın veya yeni hesap oluşturun</p>
                    </div>
                    <form method="POST">
                        <div class="form-group">
                            <input type="text" name="UserName" class="form-control" placeholder="Kullanıcı Adı" required>
                        </div>
                        <div class="form-group">
                            <input type="password" name="Password" class="form-control" placeholder="Şifre" required>
                        </div>
                        
                        <button type="submit" name="login" class="btn-login">Giriş Yap</button>
                    </form>
                    <div class="login-footer">
                        <a href="#" data-toggle="modal" data-target="#myModal">Hesap Oluştur</a> | 
                        <a href="#" data-toggle="modal" data-target="#recuperar">Şifremi Unuttum</a>
                    </div>
                </div>
                <?php else: ?>
                <?php $userdata = unserialize($_SESSION['UserData']); ?>
                <div class="login-card">
                    <div class="login-header">
                        <h2>Hoş Geldin!</h2>
                        <p><?php echo htmlspecialchars($userdata->UserName); ?></p>
                    </div>
                    <div class="text-center mb-4">
                        <img src="https://trbombom.com/images/gn_bn_khamphacoloa_210x200.fw.png"<?php echo htmlspecialchars($userdata->UserName); ?>" alt="Profil" class="rounded-circle mb-3" style="width: 150px; height: 150px; object-fit: cover; border: 4px solid #667eea;">
                        <h3><?php echo htmlspecialchars($userdata->UserName); ?></h3>
                        <p class="text-muted">Seviye 24 Oyuncu</p>
                    </div>
                    <div class="player-stats">
                        <div class="player-stat">
                            <div class="player-stat-value">24</div>
                            <div class="player-stat-label">Seviye</div>
                        </div>
                        <div class="player-stat">
                            <div class="player-stat-value">1,250</div>
                            <div class="player-stat-label">Puan</div>
                        </div>
                        <div class="player-stat">
                            <div class="player-stat-value">42</div>
                            <div class="player-stat-label">Arkadaş</div>
                        </div>
                    </div>
                    <div class="text-center mt-4">
                        <a href="play.php" class="btn-login">Oyuna Başla</a>
                        <a href="?logout=true" class="d-block mt-3 text-danger">Çıkış Yap</a>
                    </div>
                </div>
                <?php endif; ?>
            </div>
            <div class="hero-right">
                <!-- Tanıtım Listesi -->
                <div class="intro-container">
                    <!-- Sayfa 1 -->
                    <div class="intro-page active" id="page1">
                        <h3>Oyun Tanıtımı</h3>
                        <p>Tr Bombom, heyecan dolu bir savaş ve strateji oyunudur. Oyuncular, çeşitli karakterlerle arenada mücadele ederler.</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (210).png" alt="Karakter 1">
                        <p>Her karakterin kendine özgü yetenekleri ve güçlü silahları vardır. Stratejik hamlelerle rakiplerinizi alt edin!</p>
                    </div>
                    
                    <!-- Sayfa 2 -->
                    <div class="intro-page" id="page2">
                        <h3>Karakter Sınıfları</h3>
                        <p>Oyunda 5 farklı karakter sınıfı bulunmaktadır. Her sınıfın kendine özgü yetenekleri ve avantajları vardır.</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (98).png" alt="Karakter 2">
                        <p>Savaşçı, büyücü, nişancı, destek ve tank sınıflarından seçim yaparak oyun tarzınıza uygun karakteri oluşturun.</p>
                    </div>
                    
                    <!-- Sayfa 3 -->
                    <div class="intro-page" id="page3">
                        <h3>Silah ve Ekipmanlar</h3>
                        <p>Oyunda yüzlerce farklı silah ve ekipman bulunmaktadır. Her silahın kendine özgü özellikleri vardır.</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (205).png" alt="Karakter 3">
                        <p>Efsanevi silahlar toplayarak karakterinizi güçlendirin ve arenada rakiplerinize korku salın!</p>
                    </div>
                    
                    <!-- Sayfa 4 -->
                    <div class="intro-page" id="page4">
                        <h3>PvP Savaşları</h3>
                        <p>Başka oyuncularla gerçek zamanlı savaşlar yaparak yeteneklerinizi test edin. Liginde yükselerek ödüller kazanın!</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (120).png" alt="Karakter 4">
                        <p>Haftalık turnuvalara katılarak en iyi oyuncular arasında yerinizi alın ve özel ödüller kazanın!</p>
                    </div>
                    
                    <!-- Sayfa 5 -->
                    <div class="intro-page" id="page5">
                        <h3>Lonca Sistemi</h3>
                        <p>Lonca kurun veya mevcut loncalara katılın. Lonca arkadaşlarınızla birlikte boss avlayın ve lonca savaşlarına katılın.</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (45).png" alt="Karakter 5">
                        <p>Lonca depolama sistemi sayesinde kaynaklarınızı paylaşın ve birlikte güçlenin!</p>
                    </div>
                    
                    <!-- Sayfa 6 -->
                    <div class="intro-page" id="page6">
                        <h3>Etkinlikler ve Ödüller</h3>
                        <p>Günlük, haftalık ve özel etkinliklere katılarak nadir ödüller kazanın. Sezonluk geçişlerle yeni içeriklerin kilidini açın!</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (88).png" alt="Karakter 6">
                        <p>Yıl boyunca düzenlenen özel etkinlikleri kaçırmayın ve sınırlı süreli karakterlere ve silahlara sahip olun!</p>
                    </div>
                    
                    <!-- Sayfa 7 -->
                    <div class="intro-page" id="page7">
                        <h3>Haritalar ve Modlar</h3>
                        <p>Farklı haritalarda çeşitli oyun modlarında mücadele edin. Takım savaşları, serbest oyun ve daha fazlası!</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (150).png" alt="Karakter 7">
                        <p>Her haritanın kendine özgü stratejik noktaları vardır. Bu noktaları kontrol ederek savaşta avantaj elde edin!</p>
                    </div>
                    
                    <!-- Sayfa 8 -->
                    <div class="intro-page" id="page8">
                        <h3>Güncellemeler ve Yeni İçerikler</h3>
                        <p>Oyun düzenli olarak yeni karakterler, silahlar, haritalar ve etkinliklerle güncellenir. Her güncellemeyle yeni maceralar sizi bekler!</p>
                        <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (180).png" alt="Karakter 8">
                        <p>Topluluk geri bildirimleri doğrultusunda oyun sürekli geliştirilir. Sizin de fikirleriniz oyunun geleceğini şekillendirir!</p>
                    </div>
                    
                    <!-- Sayfa Kontrolleri -->
                    <div class="pagination">
                        <button id="prevBtn" onclick="changePage(-1)" disabled><i class="fas fa-chevron-left"></i></button>
                        <span id="pageIndicator">1 / 8</span>
                        <button id="nextBtn" onclick="changePage(1)"><i class="fas fa-chevron-right"></i></button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Silahlar Bölümü -->
    <section id="silahlar" class="section">
        <div class="container">
            <div class="section-title">
                <h2>Silahlar</h2>
                <p>Oyunda kullanabileceğiniz güçlü ve efsanevi silahlar</p>
            </div>
            <div class="weapons-container">
                <div class="weapon-card">
                    <img src="https://trbombom.com/images/bumerang.png" alt="Bumerang" class="weapon-image">
                    <div class="weapon-info">
                        <h3>Efsanevi Sevgi Bumerangı</h3>
                        <p>Yüksek isabet oranı ve geri sekme özelliği ile rakiplerinize korku salın.</p>
                        <div class="weapon-stats">
                            <span class="weapon-stat">Hasar: 240</span>
                            <span class="weapon-stat">Kritik: %15</span>
                            <span class="weapon-stat">Açı: 20-65</span>
                        </div>
                    </div>
                </div>
                <div class="weapon-card">
                    <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (98).png" alt="Bom" class="weapon-image">
                    <div class="weapon-info">
                        <h3>Efsanevi Antik Bambu</h3>
                        <p>Geniş alan hasarı ve yüksek patlama gücü ile kalabalık düşman gruplarını dağıtın.</p>
                        <div class="weapon-stats">
                            <span class="weapon-stat">Hasar: 265</span>
                            <span class="weapon-stat">Kritik: %5</span>
                            <span class="weapon-stat">Açı: 15-55</span>
                        </div>
                    </div>
                </div>
                <div class="weapon-card">
                    <img src="https://trbombom.com/karakterpng/Pack AiolosDDT (205).png" alt="Silah 3" class="weapon-image">
                    <div class="weapon-info">
                        <h3>Efsanevi Minotar Baltası</h3>
                        <p>Ateş elementi ile dolu bu mızrak, düşmanlarınıza yakıcı hasar verir.</p>
                        <div class="weapon-stats">
                            <span class="weapon-stat">Hasar: 280</span>
                            <span class="weapon-stat">Kritik: %10</span>
                            <span class="weapon-stat">Açı: 25-70</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Galeri Bölümü -->
    <section id="galeri" class="section" style="background-color: #0f0f1a;">
        <div class="container">
            <div class="section-title">
                <h2>Galeri</h2>
                <p>Oyun içi en güzel görüntüler ve anlar</p>
            </div>
            <div class="gallery-grid">
                <div class="gallery-item">
                    <img src="https://trbombom.com/images/Arkaplan1.jpg" alt="Galeri 1">
                    <div class="gallery-overlay">
                        <h3>Savaş Alanı</h3>
                        <p>En popüler haritalardan biri</p>
                    </div>
                </div>
                <div class="gallery-item">
                    <img src="https://lalala.photos/seed/gallery2/400/300" alt="Galeri 2">
                    <div class="gallery-overlay">
                        <h3>Ejderha Mağarası</h3>
                        <p>Zorlu boss savaşları</p>
                    </div>
                </div>
                <div class="gallery-item">
                    <img src="https://lalala.photos/seed/gallery3/400/300" alt="Galeri 3">
                    <div class="gallery-overlay">
                        <h3>Sihirli Orman</h3>
                        <p>Gizli hazineler</p>
                    </div>
                </div>
                <div class="gallery-item">
                    <img src="https://lalala.photos/seed/gallery4/400/300" alt="Galeri 4">
                    <div class="gallery-overlay">
                        <h3>Buz Kalesi</h3>
                        <p>Kış etkinlikleri</p>
                    </div>
                </div>
                <div class="gallery-item">
                    <img src="https://lalala.photos/seed/gallery5/400/300" alt="Galeri 5">
                    <div class="gallery-overlay">
                        <h3>Volkanik Dağ</h3>
                        <p>Ateş elementleri</p>
                    </div>
                </div>
                <div class="gallery-item">
                    <img src="https://lalala.photos/seed/gallery6/400/300" alt="Galeri 6">
                    <div class="gallery-overlay">
                        <h3>Su Altı Şehri</h3>
                        <p>Su elementleri</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Wiki Bölümü -->
    <section id="wiki" class="section">
        <div class="container">
            <div class="section-title">
                <h2>Oyun Wiki</h2>
                <p>Oyun hakkında detaylı bilgiler ve rehberler</p>
            </div>
            <div class="wiki-grid">
                <div class="wiki-card">
                    <h3>Karakter Sınıfları</h3>
                    <p>Oyunda 5 farklı karakter sınıfı bulunmaktadır. Her sınıfın kendine özgü yetenekleri ve avantajları vardır.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> Admin</span>
                        <span><i class="fas fa-calendar"></i> 15.05.2023</span>
                    </div>
                </div>
                <div class="wiki-card">
                    <h3>Harita Rehberi</h3>
                    <p>Tüm haritaların detaylı rehberi, stratejik noktalar ve gizli geçitler hakkında bilgiler.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> Moderator</span>
                        <span><i class="fas fa-calendar"></i> 20.05.2023</span>
                    </div>
                </div>
                <div class="wiki-card">
                    <h3>Ekipman Sistemi</h3>
                    <p>Ekipmanların seviyelendirilmesi, yükseltme yöntemleri ve nadir ekipmanların nasıl bulunacağı.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> WikiEditör</span>
                        <span><i class="fas fa-calendar"></i> 25.05.2023</span>
                    </div>
                </div>
                <div class="wiki-card">
                    <h3>Boss Rehberi</h3>
                    <p>Tüm boss'ların zayıf noktaları, saldırı desenleri ve ödülleri hakkında detaylı bilgiler.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> Oyuncu</span>
                        <span><i class="fas fa-calendar"></i> 30.05.2023</span>
                    </div> class="wiki-card">
                    <h3>Lonca Sistemi</h3>
                    <p>Lonca kurma, yönetme ve lonca savaşları hakkında bilmeniz gereken her şey.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> Admin</span>
                        <span><i class="fas fa-calendar"></i> 05.06.2023</span>
                    </div>
                </div>
                <div class="wiki-card">
                    <h3>PvP Stratejileri</h>Diğer oyunculara karşı kullanabileceğiniz en etkili stratejiler ve taktikler.</p>
                    <div class="wiki-meta">
                        <span><i class="fas fa-user"></i> Şampiyon</span>
                        <span><i class="fas fa-calendar"></i> 10.06.2023</span>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Forum Bölümü -->
    <section id="forum" class="section" style="background-color: #0f0f1a="container">
            <div class="section-title">
                <h2>Forum</h2>
                <p>Oyuncuların tartışma platformu</p>
            </div>
            <div class="forum-container">
                <div class="forum-card">
                    <div class="forum-header">
                        <h3 class="forum-title">Yeni Güncelleme Ne Zaman?</h3>
                        <div class="forum-meta">
                            <span><i class="fas fa-user"></i> Oyuncu123</span<i class="fas fa-clock"></i> 2 saat önce</span>
                        </div>
                    </div>
                    <div class="forum-content">
                        <p>Merhaba, yeni güncelleme hakkında bilgi alabileceğimiz bir tarih var mı? Özellikle yeni karakter sınıfı ne zaman gelecek?</p>
                    </div>
                    <div class="forum-footer">
                        <div class="forum-actions">
                            <button class="liked"><i class="fas 24</button>
                            <button><i class="fas fa-comment"></i> 8</button>
                        </div>
                        <a href="#" class="btn btn-sm btn-primary">Devamını Oku</a>
                    </div>
                </div>
                <div class="forum-card">
                    <div class="forum-header">
                        <h3 class="forum-title">Lonca Arıyorum</h3>
                        <div class="forum-meta">fas fa-user"></i> YeniOyuncu</span>
                            <span><i class="fas fa-clock"></i> 5 saat önce</span>
                        </div>
                    </div>
                    <div class="forum-content">
                        <p>Yeni başladım ve aktif bir lonca arıyorum. Seviyem 15 ve her gün aktif oluyorum. Discord'da da bulunuyorum.</p>
                    </div>
                    <div class="forum-footer">
                        <div class="forum-actions"><i class="fas fa-heart"></i> 12</button>
                            <button><i class="fas fa-comment"></i> 15</button>
                        </div>
                        <a href="#" class="btn btn-sm btn-primary">Devamını Oku</a>
                    </div>
                </div>
                <div class="forum-card">
                    <div class="forum-header">
                        <h3 class="forum-title">Boss Stratejileri</h3>
                        <div class="forum-meta">fas fa-user"></i> UstaOyuncu</span>
                            <span><i class="fas fa-clock"></i> 1 gün önce</span>
                        </div>
                    </div>
                    <div class="forum-content">
                        <p>Ejderha Mağarası'ndaki boss'u yenmek için kullandığım stratejiyi paylaşıyorum. Özellikle ateş elementi kullananlar için...</p>
                    </div>
                    <div class="forum-footer">
                        <div class="forum-actions">
                            <button class="liked"><i class="fas fa-heart"></i> 56</button>
                            <button><i class="fas fa-comment"></i> 23</button>
                        </div>
                        <a href="#" class="btn btn-sm btn-primary">Devamını Oku</a>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Oyuncu Profilleri Bölümü -->
    <section id="oyuncular" class="section">
        <div class="container">
            <div class="section-title">
                <h2>Oyuncu Profilleri</h2>
                <p>En başarılı oyuncuların profilleri</p>
            </div>
            <div class="players-grid">
                <div class="player-card">
                    <img src="https://lalala.photos/seed/player1/300/200" alt="Oyuncu 1" class="player-avatar">
                    <div class="player-info">
                        <h3 class="player-name">DragonSlayer</h3>
                        <p class="player-title">Ejaderha Avı Uzmanı</p>
                        <div class="player-stats">
                            <div class="player-stat">
                                <div class="player-stat-value">85</div>
                                <div class="player-stat-label">Seviye</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">12,450</div>
                                <div class="player-stat-label">Puan</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">156</div>
                                <div class="player-stat-label">Arkadaş</div>
                            </div>
                        </div>
                        <p class="player-bio">Oyunda 3 yıldır aktifim. Boss avlamak ve lonca savaşlarına katılmak benim hobim.</p>
                    </div>
                </div>
                <div class="player-card">
                    <img src="https://lalala.photos/seed/player2/300/200" alt="Oyuncu 2" class="player-avatar">
                    <div class="player-info">
                        <h3 class="player-name">MagicQueen</h3>
                        <p class="player-title">Büyü Ustası</p>
                        <div class="player-stats">
                            <div class="player-stat">
                                <div class="player-stat-value">72</div>
                                <div class="player-stat-label">Seviye</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">9,820</div>
                                <div class="player-stat-label">Puan</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">89</div>
                                <div class="player-stat-label">Arkadaş</div>
                            </div>
                        </div>
                        <p class="player-bio">Büyücü sınıfını seviyorum. Özellikle element büyülerinde uzmanım. Yeni başlayanlara yardım etmeyi severim.</p>
                    </div>
                </div>
                <div class="player-card">
                    <img src="https://lalala.photos/seed/player3/300/200" alt="Oyuncu 3" class="player-avatar">
                    <div class="player-info">
                        <h3 class="player-name">ShadowHunter</h3>
                        <p class="player-title">Suikastçı</p>
                        <div class="player-stats">
                            <div class="player-stat">
                                <div class="player-stat-value">68</div>
                                <div class="player-stat-label">Seviye</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">8,750</div>
                                <div class="player-stat-label">Puan</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">64</div>
                                <div class="player-stat-label">Arkadaş</div>
                            </div>
                        </div>
                        <p class="player-bio">Hızlı ve sessiz hareket etmeyi severim. PvP savaşlarında uzmanım. Lonca savaşlarında strateji yapıyorum.</p>
                    </div>
                </div>
                <div class="player-card">
                    <img src="https://lalala.photos/seed/player4/300/200" alt="Oyuncu 4" class="player-avatar">
                    <div class="player-info">
                        <h3 class="player-name">IronGuardian</h3>
                        <p class="player-title">Savunma Uzmanı</p>
                        <div class="player-stats">
                            <div class="player-stat">
                                <div class="player-stat-value">78</div>
                                <div class="player-stat-label">Seviye</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">10,320</div>
                                <div class="player-stat-label">Puan</div>
                            </div>
                            <div class="player-stat">
                                <div class="player-stat-value">112</div>
                                <div class="player-stat-label">Arkadaş</div>
                            </div>
                        </div>
                        <p class="player-bio">Tank sınıfını oynuyorum. Takım arkadaşlarımı korumak benim için en önemli şey. Lonca lideriyim.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Kayıt Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Hesap Oluştur</h4>
                </div>
                <form method="POST">
                    <div class="modal-body">
                        <div class="form-group">
                            <label>Kullanıcı Adı</label>
                            <input name="Username" type="text" class="form-control" required>
                        </div>
                        <div class="form-row">
                            <div class="col">
                                <label>Şifre</label>
                                <input name="password" type="password" class="form-control" required>
                            </div>
                            <div class="col">
                                <label>Şifre Onayı</label>
                                <input name="Repassword" type="password" class="form-control" required>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="col">
                                <label>Oyun İçi İsim</label>
                                <input name="Nickname" type="text" class="form-control" required>
                            </div>
                            <div class="col">
                                <label>Cinsiyet</label>
                                <select name="sex" class="form-control">
                                    <option value="1">Erkek</option>
                                    <option value="0">Kadın</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>E-posta Adresi</label>
                            <input type="email" name="Email" class="form-control" required>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">İptal</button>
                        <button type="submit" name="register" class="btn btn-primary">Kayıt Ol</button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- Şifremi Unuttum Modal -->
    <div class="modal fade" id="recuperar" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Şifre Sıfırlama</h4>
                </div>
                <div class="modal-body">
                    <p>Şifrenizi sıfırlamak için kayıtlı e-posta adresinizi girin.</p>
                    <div class="form-group">
                        <label>E-posta</label>
                        <input type="email" class="form-control" placeholder="E-posta adresinizi girin">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">İptal</button>
                    <button type="button" class="btn btn-primary">Gönder</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Scriptler -->
    <script src="./Assets/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="./Assets/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="./Assets/js/webtoolkit.md5.js"></script>
    <script src="./Assets/js/jquery.corner.js"></script>
    <script src="./Assets/js/jquery-ui-1.8.21.custom.js"></script>
    <script src="./Assets/js/jquery.ui.button.js"></script>
    <script src="./Assets/js/password_ddt.js"></script>
    <script src="./Assets/js/promo-top-bar.js"></script>

    <script>
        // Sayfa değiştirme fonksiyonu
        let currentPage = 1;
        const totalPages = 8;
        
        function showPage(pageNum) {
            // Tüm sayfaları gizle
            for (let i = 1; i <= totalPages; i++) {
                document.getElementById(`page${i}`).classList.remove('active');
            }
            
            // İstenen sayfayı göster
            document.getElementById(`page${pageNum}`).classList.add('active');
            
            // Sayfa göstergesini güncelle
            document.getElementById('pageIndicator').textContent = `${pageNum} / ${totalPages}`;
            
            // Önceki/sonraki butonlarını güncelle
            document.getElementById('prevBtn').disabled = pageNum === 1;
            document.getElementById('nextBtn').disabled = pageNum === totalPages;
            
            currentPage = pageNum;
        }
        
        function changePage(direction) {
            const newPage = currentPage + direction;
            if (newPage >= 1 && newPage <= totalPages) {
                showPage(newPage);
            }
        }
        
        $(document).ready(function() {
            // Yumuşak kaydırma
            $('a[href^="#"]').on('click', function(event) {
                var target = $(this.getAttribute('href'));
                if (target.length) {
                    event.preventDefault();
                    $('html, body').stop().animate({
                        scrollTop: target.offset().top - 80
                    }, 1000);
                }
            });

            // Forum beğeni butonları
            $('.forum-actions button').on('click', function() {
                if ($(this).hasClass('liked')) {
                    $(this).removeClass('liked');
                    var count = parseInt($(this).text().trim());
                    $(this).html('<i class="fas fa-heart"></i> ' + (count - 1));
                } else {
                    $(this).addClass('liked');
                    var count = parseInt($(this).text().trim());
                    $(this).html('<i class="fas fa-heart"></i> ' + (count + 1));
                }
            });
            
            // Animasyonları tetikle
            const observerOptions = {
                root: null,
                rootMargin: '0px',
                threshold: 0.1
            };

            const observer = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.style.animationPlayState = 'running';
                        observer.unobserve(entry.target);
                    }
                });
            }, observerOptions);

            // Animasyonlu elemanları gözlemle
            document.querySelectorAll('.weapon-card, .gallery-item, .wiki-card, .forum-card, .player-card').forEach(el => {
                el.style.animationPlayState = 'paused';
                observer.observe(el);
            });
            
            // Parallax efekti
            $(window).on('scroll', function() {
                var scrolled = $(window).scrollTop();
                $('.hero-bg').css('transform', 'translateY(' + (scrolled * 0.5) + 'px)');
            });
            
            // Rastgele parçacık animasyonu
            function createParticle() {
                const particle = document.createElement('div');
                particle.className = 'particle';
                particle.style.left = Math.random() * 100 + '%';
                particle.style.top = Math.random() * 100 + '%';
                particle.style.width = Math.random() * 10 + 5 + 'px';
                particle.style.height = particle.style.width;
                particle.style.backgroundColor = `rgba(${Math.random() * 100 + 100}, ${Math.random() * 100 + 100}, 255, ${Math.random() * 0.5 + 0.2})`;
                particle.style.borderRadius = '50%';
                particle.style.position = 'fixed';
                particle.style.pointerEvents = 'none';
                particle.style.zIndex = '1';
                particle.style.opacity = '0';
                particle.style.transform = 'translateY(0)';
                
                document.body.appendChild(particle);
                
                // Animasyon
                setTimeout(() => {
                    particle.style.transition = 'all 2s ease-out';
                    particle.style.opacity = '1';
                    particle.style.transform = `translateY(${Math.random() * 200 - 100}px) translateX(${Math.random() * 200 - 100}px)`;
                }, 10);
                
                // Temizleme
                setTimeout(() => {
                    particle.style.transition = 'opacity 1s ease-out';
                    particle.style.opacity = '0';
                    setTimeout(() => {
                        document.body.removeChild(particle);
                    }, 1000);
                }, 2000);
            }
            
            // Belirli aralıklarla parçacık oluştur
            setInterval(createParticle, 300);
        });
    </script>

</body>
</html>