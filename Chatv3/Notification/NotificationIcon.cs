using Chatv3.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chatv3.Notification
{
    public class NotificationIcon
    {
        private NotifyIcon notifyIcon;

        public NotificationIcon()
        {
            notifyIcon = new NotifyIcon() { Icon = Resources.off_tTm_icon, Visible = true }; 
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
        }

        public void TurnOn()
        {
            notifyIcon.Icon = Resources.on_8fN_icon;
        }

        public void TurnOff()
        {
            notifyIcon.Icon = Resources.off_tTm_icon;
        }


    }
}
