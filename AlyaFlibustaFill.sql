EXEC AddGenre 'Epic Novel'; -- Роман-эпопея
EXEC AddGenre 'Novel'; -- Роман
EXEC AddGenre 'Science Fiction'; -- Фантастика
EXEC AddGenre 'Detective'; -- Детектив
EXEC AddGenre 'Adventure'; -- Приключения
EXEC AddGenre 'Thriller'; -- Триллер
EXEC AddGenre 'Mystery'; -- Мистика
EXEC AddGenre 'Historical'; -- Исторический
EXEC AddGenre 'Drama'; -- Драма
EXEC AddGenre 'Poetry'; -- Поэзия

EXEC AddAuthor 'Lev', 'Tolstoy'; -- Лев Толстой
EXEC AddAuthor 'Fyodor', 'Dostoevsky'; -- Федор Достоевский
EXEC AddAuthor 'George', 'Orwell'; -- Джордж Оруэлл
EXEC AddAuthor 'Agatha', 'Christie'; -- Агата Кристи
EXEC AddAuthor 'Arthur', 'Conan Doyle'; -- Артур Конан Дойл
EXEC AddAuthor 'Jules', 'Verne'; -- Жюль Верн
EXEC AddAuthor 'Haruki', 'Murakami'; -- Харуки Мураками
EXEC AddAuthor 'Stephen', 'King'; -- Стивен Кинг
EXEC AddAuthor 'Victor', 'Hugo'; -- Виктор Гюго
EXEC AddAuthor 'Alexander', 'Pushkin'; -- Александр Пушкин

--exec AddUserSimple 'Administrator', '0451'

EXEC AddBook 1, 'War and Peace', 1, 'A historical epic novel', 'Path', 'IMG'; -- Война и мир
EXEC AddBook 1, 'Crime and Punishment', 2, 'A psychological drama', 'Path', 'IMG'   ; -- Преступление и наказание
EXEC AddBook 1, '1984', 3, 'A dystopian science fiction novel', 'Path', 'IMG'   ; -- 1984
EXEC AddBook 1, 'Murder on the Orient Express', 4, 'A detective novel with a mysterious twist', 'Path', 'IMG'   ; -- Убийство в Восточном экспрессе
EXEC AddBook 1, 'Sherlock Holmes', 5, 'A collection of detective stories', 'Path', 'IMG'   ; -- Шерлок Холмс
EXEC AddBook 1, '20,000 Leagues Under the Sea', 6, 'A science fiction adventure novel', 'Path', 'IMG'   ; -- 20,000 лье под водой
EXEC AddBook 1, 'Norwegian Wood', 7, 'A tragic romance and drama', 'Path', 'IMG'   ; -- Норвежский лес
EXEC AddBook 1, 'The Shining', 8, 'A psychological horror and thriller', 'Path', 'IMG'   ; -- Сияние
EXEC AddBook 1, 'Les Misérables', 9, 'A historical drama of French society', 'Path', 'IMG'   ; -- Отверженные
EXEC AddBook 1, 'Eugene Onegin', 10, 'A romantic narrative poem', 'Path', 'IMG'    ; -- Евгений Онегин


--/*Доделать связываение книг, добавить связывание книг через названия*/
EXEC AddNameGenre2NameBook 'War and Peace', 'Epic Novel' ; -- Война и мир - Роман-эпопея

EXEC AddNameGenre2NameBook 'Crime and Punishment', 'Novel'; -- Преступление и наказание - Роман
EXEC AddNameGenre2NameBook '1984', 'Science Fiction'; -- 1984 - Фантастика
EXEC AddNameGenre2NameBook  'Murder on the Orient Express', 'Detective'; -- Убийство в Восточном экспрессе - Детектив
EXEC AddNameGenre2NameBook 'Sherlock Holmes', 'Detective'; -- Шерлок Холмс - Детектив
EXEC AddNameGenre2NameBook '20,000 Leagues Under the Sea', 'Adventure'; -- 20,000 лье под водой - Приключения
EXEC AddNameGenre2NameBook 'Norwegian Wood', 'Novel'; -- Норвежский лес - Роман
EXEC AddNameGenre2NameBook 'The Shining', 'Thriller'; -- Сияние - Триллер
EXEC AddNameGenre2NameBook  'Les Misérables', 'Historical'; -- Отверженные - Исторический
EXEC AddNameGenre2NameBook 'Eugene Onegin', 'Poetry'; -- Евгений Онегин - Поэзия
