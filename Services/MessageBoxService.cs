using System.Windows;

namespace UserPaymentsDesktopApp.Services
{
    /// <summary>
    /// Реализует статические методы для обратной связи.
    /// </summary>
    public static class MessageBoxService
    {
        /// <summary>
        /// Информирует пользователя.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Inform(string message)
        {
            _ = MessageBox.Show(message,
                            "Информация",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        /// <summary>
        /// Предупреждает пользователя.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public static void Warn(string message)
        {
            _ = MessageBox.Show(message,
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
        }

        /// <summary>
        /// Спрашивает пользователя.
        /// </summary>
        /// <param name="message">Вопрос</param>
        public static bool Ask(string question)
        {
            return MessageBox.Show(question,
                            "Вопрос",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question)
                == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Информирует пользователя об ошибке.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void InformError(string message)
        {
            _ = MessageBox.Show(message,
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
