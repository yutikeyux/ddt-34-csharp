using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LauncherGHU
{
	[ComVisible(true)]
	public class ScriptManager
	{
		private Form form_0;

		internal static ScriptManager oL233oUoL7DrBTrGOhX;

		public ScriptManager(Form form)
		{
			form_0 = form;
		}

		public void ShowMessage(object obj)
		{
			MessageBox.Show(obj.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			form_0.Close();
		}

		public void ShowMessageAndBackLogin(object obj)
		{
			MessageBox.Show(obj.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			if (form_0 is PlayGameFrmFIB)
			{
				(form_0 as PlayGameFrmFIB).CloseAndBackLogin(isLogout: true);
			}
			if (form_0 is PlayGameFrm2FIB)
			{
				(form_0 as PlayGameFrm2FIB).CloseAndBackLogin(isLogout: true);
			}
		}

		public void LoginGame(object username, object password)
		{
			LoginMgr.Login(username.ToString(), password.ToString(), 0);
            //if (form_0.Owner is LoginFrm)
            //{
            //	(form_0.Owner as LoginFrm).OpenPlayGame();
            //}
            if (form_0.Owner is formLogin)
            {
                //(form_0.Owner as formLogin).OpenPlayGame();
            }
            form_0.Close();
		}

		public void SessionExpired()
		{
			MessageBox.Show("Đăng nhập hết hạn. Vui lòng đăng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			if (form_0 is PlayGameFrmFIB)
			{
				(form_0 as PlayGameFrmFIB).CloseAndBackLogin(isLogout: true);
			}
			if (form_0 is PlayGameFrm2FIB)
			{
				(form_0 as PlayGameFrm2FIB).CloseAndBackLogin(isLogout: true);
			}
		}

		public void BackLogin()
		{
			if (form_0 is PlayGameFrmFIB)
			{
				(form_0 as PlayGameFrmFIB).CloseAndBackLogin(isLogout: true);
			}
			if (form_0 is PlayGameFrm2FIB)
			{
				(form_0 as PlayGameFrm2FIB).CloseAndBackLogin(isLogout: true);
			}
		}

		public void RemoteWebsite(string url)
		{
			ControlMgr.OpenWebsite(url);
		}

		internal static bool rwayXFUVJxDHIM5R9eG()
		{
			return oL233oUoL7DrBTrGOhX == null;
		}
	}
}
