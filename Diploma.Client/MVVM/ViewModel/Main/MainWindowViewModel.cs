using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;

namespace Diploma.Client.MVVM.ViewModel.Main
{
    /// <summary>
    /// Вьюмодель основного окна
    /// </summary>
    public sealed class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Авторизованный пользователь
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Чаты
        /// </summary>
        public List<Chat> Chats { get; }

        /// <summary>
        /// Сообщения чата
        /// </summary>
        public List<Message> Messages { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindowViewModel()
        {
            User = new User
            {
                Id = Guid.Empty,
                Name = "Дмитрий Тришин",
                Email = "daltrishin@sberbank.ru"
            };
            
            Chats = new List<Chat>
            {
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Алексей Терешков"
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Алексей Веселов"
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Питер Паркер"
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Name = "Иван Морозов"
                },
            };

            Messages = new List<Message>
            {
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "Lalka",
                    DateCreate = DateTime.UtcNow
                },
            };
        }
    }
}