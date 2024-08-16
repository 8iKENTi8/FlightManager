using Microsoft.Win32;
using System;

namespace FlightManager.Utils.Helpers
{
    public static class DialogHelper
    {
        // Показывает диалоговое окно для открытия файла
        public static string? ShowOpenFileDialog(string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            // Если пользователь подтвердил выбор файла, возвращаем путь, иначе возвращаем null
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        // Показывает диалоговое окно для сохранения файла
        public static string? ShowSaveFileDialog(string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", string defaultFileName = "data")
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                FileName = defaultFileName
            };

            // Если пользователь подтвердил выбор файла, возвращаем путь, иначе возвращаем null
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
    }
}
