using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Diploma.Client.Core.MVVM.ViewModel;
using Diploma.Client.MVVM.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diploma.Client.MVVM.ViewModel.Main
{
    /// <summary>
    /// Вьюмодель основного окна чата
    /// </summary>
    public sealed class ChatPageViewModel : ViewModelBase
    {
        const string GET_USERS_URL = "https://localhost:5001/users";
        
        /// <summary>
        /// Поиск пользователей
        /// </summary>
        public List<User> SearchUsers { get; set; }

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
        public ChatPageViewModel()
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
                    Text = "ППРиветППРиветППРиветППРиветППРиветППРиветППРиветППРиветППРивет",
                    DateCreate = DateTime.UtcNow
                },
                new Message
                {
                    Username = "newmersedez",
                    Text = "LalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalka",
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
                    Text = "LalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalkaLalka",
                    DateCreate = DateTime.UtcNow
                },
            };
        }
    }
}