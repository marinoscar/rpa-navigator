using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luval.rpa.navigator
{
    public static class UiHelper
    {
        public static void ExecuteAction(this Form form, Func<object> action, Action<object> onSucces, Action<Exception> onError)
        {
            var current = form.Cursor;
            form.Cursor = Cursors.WaitCursor;
            object res = null;
            try
            {
                res = action();
            }
            catch (Exception ex)
            {
                form.Cursor = current;
                ShowExceptionMessage(ex);
                onError?.Invoke(ex);
            }
            finally
            {
                form.Cursor = current;
            }
            onSucces?.Invoke(res);
        }

        public static void ShowExceptionMessage(this Form form, Exception ex)
        {
            ShowExceptionMessage(ex);
        }

        public static void ShowExceptionMessage(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
