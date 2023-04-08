using System;

namespace Diploma.Client.MVVM.Model
{
    public static class AuthEventExtensions
    {
        public static string GetComment(this AuthEventType eventType)
        {
            return eventType switch
            {
                AuthEventType.LOGIN => "Уже есть аккаунт?",
                AuthEventType.REGISTRATION => "Еще нет аккаунта?",
                _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
            };
        }
        
        public static string GetAction(this AuthEventType eventType)
        {
            return eventType switch
            {
                AuthEventType.LOGIN => "Войти",
                AuthEventType.REGISTRATION => "Зарегистрироваться",
                _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
            };
        }
    }
}