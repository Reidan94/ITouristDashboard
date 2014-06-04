using ITourist.Models.LogicModels.Services;

namespace ITourist.Models.DataModels
{
    public class ProcessResult : Translatable
    {
        public int Id { get; private set; }
        public bool Succeeded { get; private set; }
        public Translation Message { get; private set; }
        public int? AffectedObjectId { get; set; }

        public ProcessResult(int id, bool succeeded, Translation message, int? affectedObjectId = null)
        {
            Id = id;
            Succeeded = succeeded;
            Message = message;
            AffectedObjectId = affectedObjectId;
        }

        public string GetMessage(Culture culture)
        {
            return GetTranslation(culture, Message);
        }
    }
    public class ProcessResults
    {
        static readonly ProcessResult[] Results =
        {
            new ProcessResult(0, false, new Translation("Invalid email","Неверный адрес электонной почты")), 
            new ProcessResult(1, false, new Translation("Invalid password", "Неверный пароль")), 
            new ProcessResult(2, true, new Translation("Login is successful","Вход выполнен успешно")),
            new ProcessResult(3, false, new Translation("Invalid author","Неверный автор")),
            new ProcessResult(4, true, new Translation("Route created successfully", "Маршрут успешно создан")), 
            new ProcessResult(5, false, new Translation("CheckIn not found","Чекин не найден")), 
            new ProcessResult(6, true, new Translation("Checkin is successful","Чекин выполнен успешно")), 
            new ProcessResult(7, false, new Translation("User not found","Пользователь не найден")), 
            new ProcessResult(8, false, new Translation("Route not found","Маршрут не найден")), 
            new ProcessResult(9, true, new Translation("Route deleted successfully","Маршрут успешно удален")), 
            new ProcessResult(10, false, new Translation("Ivalid translations","Неверные переводы")),
            new ProcessResult(11, false, new Translation("Unexpected error occurred","Возникла непредвиденная ошибка")),
            new ProcessResult(12, false, new Translation("Too few checkpoints","Слишком мало чекпоинтов")),
            new ProcessResult(13, false, new Translation("Place is already checked","В этом месте уже выполнялся чекин")),
            new ProcessResult(14, false, new Translation("Experience is low (" + StaticSettings.MinUserExperince + " points needed)","Мало опыта (нужно " + StaticSettings.MinUserExperince + " очков)")),
            new ProcessResult(15, false, new Translation("Route is already public","Маршрут уже публичный")),
            new ProcessResult(16, true, new Translation("Route is public now","Маршрут теперь публичный")),
            new ProcessResult(17, false, new Translation("Route is not public","Маршрут не является публичным")),
            new ProcessResult(18, true, new Translation("Bookmark added","Закладка добавлена")),
            new ProcessResult(19, false, new Translation("Bookmark not found","Закладка не найдена")),
            new ProcessResult(20, false, new Translation("Route is already in bookmarks","Маршрут уже есть в закладках")),
            new ProcessResult(21, false, new Translation("Cannot remove bookmark of current route","Нельзя удалять закладку на текущий маршрут")),
            new ProcessResult(22, true, new Translation("Bookmark removed successfully","Закладка успешно удалена")),
            new ProcessResult(23, false, new Translation("Another route is tracking","Идет трекинг другого маршрута")),
            new ProcessResult(24, true, new Translation("Tracking started","Трекинг запущен")),
            new ProcessResult(25, false, new Translation("Tracking is not started","Трекинг не запущен")),
            new ProcessResult(26, true, new Translation("Tracking finished","Трекинг завершен")),
            new ProcessResult(27, true, new Translation("Tracking aborted", "Трекинг прерван")), 
            new ProcessResult(28, false, new Translation("Checkpoint not found","Чекпоинт не найден")), 
            new ProcessResult(29, false, new Translation("Previous chekin not found","Предыдущий чекпоинт не найден")),
            new ProcessResult(30, false, new Translation("User with such email exists","Пользователь с таким адресом электронной почты уже существует")),
            new ProcessResult(31, false, new Translation("Invalid image format","Неверный формат изображения")), 
            new ProcessResult(32, false,new Translation("Registration error","Ошибка регистрации")), 
            new ProcessResult(33, true,new Translation("User registered","Пользователь зарегестирован")),
            new ProcessResult(34, false,new Translation("Passwords do not match","Пароли не совпадают")),
            new ProcessResult(35, false,new Translation("Comment cannot be epmty","Комменарий не может быть пустым")),
            new ProcessResult(36, false,new Translation("Country already exists","Страна с таким именем уже существует")),
            new ProcessResult(37, true,new Translation("Country successfully added ","Страна была успешно добавлена ")),
            new ProcessResult(38, false,new Translation("No such country ","Запрашиваемой страны не существует ")),
            new ProcessResult(39, true,new Translation("Country was successfully edited ","Страна была успешно отредактирована ")),
            new ProcessResult(40, true,new Translation("Country was deleted ","Страна была удалена ")),
            new ProcessResult(41, false,new Translation("Region already exists","Регион с таким именем уже существует")),
            new ProcessResult(42, true,new Translation("Region successfully added ","Регион был успешно добавлен ")),
            new ProcessResult(43, true,new Translation("Region successfully edited ","Регион был успешно отредактирован ")),
            new ProcessResult(44, true,new Translation("Region was deleted ","Регион был удален ")),
            new ProcessResult(45, false,new Translation("No such region","Запрашиваемого региона не существует ")),
            new ProcessResult(46, false, new Translation("Place already exists", "Место уже существует")),
            new ProcessResult(47, false, new Translation("No such place", "Место не существует")),
            new ProcessResult(48, true, new Translation("Place was deleted ", "Место было удалено ")),
            new ProcessResult(49, true, new Translation("Place was succesfully edited", "Место было успешно отредактировано")),
            new ProcessResult(50, true, new Translation("Place was succesfully added", "Место было успешно добавлено")),
            new ProcessResult(51, true, new Translation("Image was succesfully added", "Риснуок было успешно добавлен")),
            new ProcessResult(52, false, new Translation("Photo not existing", "Риснуок не существует")),
            new ProcessResult(53, true, new Translation("Photo was deleted", "Фото было удалено")),
            new ProcessResult(54, false, new Translation("Comment not existing", "Комментарий не был найден ")),
            new ProcessResult(55, true, new Translation("Comment was deleted", "Комментарий был удален")),
            new ProcessResult(56, true, new Translation("User status was changed", "Статус пользователя был изменен")),
            new ProcessResult(57, false,new Translation("Place not found","Место не найдено")),
            new ProcessResult(58,true, new Translation("Comment added successfully","Комментарий успешно добавлен")),
            new ProcessResult(59, false,new Translation("Title cannot be empty","Название не может быть пустым")),
            new ProcessResult(60, true,new Translation("Photo added","Фото добавлено"))
        };

        public static ProcessResult GetById(int id = -1)
        {
            if (id < 0 || id > Results.Length) return null;
            return Results[id];
        }

        public static ProcessResult InvalidEmail
        {
            get { return Results[0]; }
        }

        public static ProcessResult InvalidPassword
        {
            get { return Results[1]; }
        }

        public static ProcessResult LoginSuccessful
        {
            get { return Results[2]; }
        }

        public static ProcessResult InvalidAuthor
        {
            get { return Results[3]; }
        }

        public static ProcessResult RouteCreated
        {
            get { return Results[4]; }
        }

        public static ProcessResult CheckInNotFound
        {
            get { return Results[5]; }
        }

        public static ProcessResult CheckInSuccessful
        {
            get { return Results[6]; }
        }

        public static ProcessResult UserNotFound
        {
            get { return Results[7]; }
        }

        public static ProcessResult RouteNotFound
        {
            get { return Results[8]; }
        }

        public static ProcessResult RouteDeleted
        {
            get { return Results[9]; }
        }

        public static ProcessResult InvalidTranslations
        {
            get { return Results[10]; }
        }

        public static ProcessResult Error
        {
            get { return Results[11]; }
        }

        public static ProcessResult TooFewCheckPoints
        {
            get { return Results[12]; }
        }

        public static ProcessResult CheckinAlreadyChecked
        {
            get { return Results[13]; }
        }

        public static ProcessResult LowExperience
        {
            get { return Results[14]; }
        }

        public static ProcessResult RouteAlreadyPublic
        {
            get { return Results[15]; }
        }

        public static ProcessResult RouteIsPublicNow
        {
            get { return Results[16]; }
        }

        public static ProcessResult RouteIsNotPublic
        {
            get { return Results[17]; }
        }

        public static ProcessResult BookmarkAdded
        {
            get { return Results[18]; }
        }

        public static ProcessResult BookmarkNotFound
        {
            get { return Results[19]; }
        }

        public static ProcessResult BookmarkExists
        {
            get { return Results[20]; }
        }

        public static ProcessResult CannotRemoveCurrentBookmark
        {
            get { return Results[21]; }
        }

        public static ProcessResult BookmarkRemoved
        {
            get { return Results[22]; }
        }

        public static ProcessResult AnotherRouteIsTracking
        {
            get { return Results[23]; }
        }

        public static ProcessResult TrackingStarted
        {
            get { return Results[24]; }
        }

        public static ProcessResult RouteIsNotTracking
        {
            get { return Results[25]; }
        }

        public static ProcessResult TrackingStopped
        {
            get { return Results[26]; }
        }

        public static ProcessResult TrackingAborted
        {
            get { return Results[27]; }
        }

        public static ProcessResult CheckPointNotFound
        {
            get { return Results[28]; }
        }

        public static ProcessResult InvalidPreviousCheckIn
        {
            get { return Results[29]; }
        }

        public static ProcessResult UserWithSuchEmailExists
        {
            get { return Results[30]; }
        }

        public static ProcessResult InvalidImageFormat
        {
            get { return Results[31]; }
        }

        public static ProcessResult RegistrationError
        {
            get { return Results[32]; }
        }

        public static ProcessResult UserRegistered
        {
            get { return Results[33]; }
        }

        public static ProcessResult PasswordsNotMatch
        {
            get { return Results[34]; }
        }

        public static ProcessResult CommentCannotBeEmpty
        {
            get { return Results[35]; }
        }

        public static ProcessResult CountryAlreadyExists
        {
            get { return Results[36]; }
        }

        public static ProcessResult CountryAdded
        {
            get { return Results[37]; }
        }

        public static ProcessResult NoSuchCountry
        {
            get { return Results[38]; }
        }

        public static ProcessResult CountryEdited
        {
            get { return Results[39]; }
        }

        public static ProcessResult CountryDeleted
        {
            get { return Results[40]; }
        }

        public static ProcessResult RegionAlreadyExists
        {
            get { return Results[41]; }
        }

        public static ProcessResult RegionAdded
        {
            get { return Results[42]; }
        }

        public static ProcessResult RegionEdited
        {
            get { return Results[43]; }
        }

        public static ProcessResult RegionDeleted
        {
            get { return Results[44]; }
        }

        public static ProcessResult NoSuchRegion
        {
            get { return Results[45]; }
        }

        public static ProcessResult PlaceAlreadyExists
        {
            get { return Results[46]; }
        }

        public static ProcessResult NoSucPlace
        {
            get { return Results[47]; }
        }

        public static ProcessResult PlaceDeleted
        {
            get { return Results[48]; }
        }

        public static ProcessResult PlaceEdited
        {
            get { return Results[49]; }
        }

        public static ProcessResult PlaceAdded
        {
            get { return Results[50]; }
        }

        public static ProcessResult ImageWasSuccessfullyAdded
        {
            get { return Results[51]; }
        }

        public static ProcessResult PhotoNotExisting
        {
            get { return Results[52]; }
        }

        public static ProcessResult PhotoWasDeleted
        {
            get { return Results[53]; }
        }

        public static ProcessResult CommentNotFound
        {
            get { return Results[54]; }
        }

        public static ProcessResult CommentWasDeleted
        {
            get { return Results[55]; }
        }

        public static ProcessResult UserEdited
        {
            get { return Results[56]; }
        }

        public static ProcessResult PlaceNotFound
        {
            get { return Results[57]; }
        }

        public static ProcessResult CommentAdded
        {
            get { return Results[58]; }
        }

        public static ProcessResult TitleCannotBeEmpty
        {
            get { return Results[59]; }
        }

        public static ProcessResult PhotoAdded
        {
            get { return Results[60]; }
        }
    }
}