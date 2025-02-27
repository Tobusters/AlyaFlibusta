alter procedure AddGenre
@GenreName varchar(50) 
as

if @GenreName not in (select GenreName from Genre)
begin
INSERT into Genre(GenreName) values(@GenreName)
end
go
alter proc ShowGenreName
as
select GenreName from Genre
go
alter proc ShowGenreId
as
select Id from Genre
go
alter proc ShowGenre
as
select * from Genre
go
alter proc DeleteGenreByName
@GenreName varchar(100)
as
delete from Genre where GenreName = @GenreName
go
alter PROCEDURE GetGenreIdByName
    @GenreName NVARCHAR(50),
    @NeededId INT OUTPUT
AS
BEGIN
    SELECT @NeededId = Id
    FROM Genre
    WHERE GenreName = @GenreName
END
go
alter procedure AddAuthor
@AuthorFirstName varchar(50), 
@AuthorSecondName varchar(50)
as
if (@AuthorFirstName not in (select FirstName from Author)) and  (@AuthorSecondName not in (select LastName from Author))
begin
INSERT into Author(FirstName, LastName) values(@AuthorFirstName, @AuthorSecondName)
end
go
alter procedure ShowAuthor
as
select * from Author
go
alter procedure ShowAuthorFullName
as
select FirstName + ' ' +  LastName from Author
go
alter procedure ShowAuthorFullNameId
as
select FirstName + ' ' +  LastName, Id from Author
go
alter procedure ShowAuthorShortName
as
select LEFT(FirstName, 1) + '. '+ LastName from Author
go
alter procedure ShowAuthorShortNameId
as
select LEFT(FirstName, 1) + '. '+ LastName, Id from Author
go
alter proc GetBookIdByDate
@BookDate datetime
as
declare @IdBook int
set @IdBook = (select id from BOOKS where DateOfUpload = @BookDate)
return @IdBook
go
alter proc GetBookIdByName
@BookName nvarchar(100),
@NeededId INT OUTPUT
as
    SELECT @NeededId = Id
    FROM BOOKS
    WHERE BookName = @BookName

go
alter proc LoadedBookByUser
@BookID int,
@UserID int
as
INSERT into PersonallyLoadedBooks(UserID, BookID) values(@UserID, @BookID)
go
alter proc AddBook
@UserId int,
@BookName nvarchar(100),
@AuthorId int,
@BookDescription varchar(100),
@Filepath varchar(200),
@BookIMG varchar(200)
as
declare @DateOfUpload datetime = GETDATE()
if @BookName not in (select BookName from BOOKS)
begin
insert into BOOKS(BookName, AuthorId, BookDescription, DateOfUpload, Filepath, BookIMG) values(@BookName, @AuthorId, @BookDescription,@DateOfUpload,@Filepath, @BookIMG)
end
declare @BookIDCreated int = (select MAX(Id) from BOOKS)
exec LoadedBookByUser @BookIDCreated, @UserId
go
alter proc AddUser
@Login nvarchar(100),
@Email varchar(100),
@Password varchar(200),
@NickName nvarchar(50),
@AvatarIMG varchar(200)
as
insert into USERS(Login, Password, Email, NickName, AvatarIMG) values(@Login, @Password, @Email, @NickName, @AvatarIMG)
go
alter proc AddUserSimple
@Login nvarchar(100),
@Password varchar(200)
as
exec AddUser @Login, @Password, '', '', ''
go
alter proc GetUserByLogin
@Login nvarchar(100)
as
select * from USERS where Login=@Login
go
alter proc GetUserById
@Id int
as
select * from USERS where Id=@Id
go
alter procedure AddGenre2Book
@BookID int,
@GenreID int
as
INSERT into G2B(BookID, GenreID) values(@BookID, @GenreID)
go
alter procedure AddNameGenre2NameBook
@BookName nvarchar(100),
@GenreName varchar(50)
as
declare @GID int 
declare @BID int

set @GID = (select Id from Genre where GenreName = @GenreName)
set @BID = (select Id from BOOKS where BookName = @BookName)
insert into G2B(BookID, GenreID) values (@BID, @GID)
go
alter proc ShowGenre2Book
as
select Count(*)  from G2B
select *  from G2B
go
alter proc ShowSimpleBooksForViewTable
as
select BOOKS.Id, BookName as 'Name', FirstName + ' ' + LastName as 'Author' from BOOKS  JOIN Author on BOOKS.AuthorId = Author.Id
go
alter proc ShowLoadedBooksByUserId
@Id int
as 
select * from PersonallyLoadedBooks 
go
/*

delete from PersonallyLoadedBooks
delete from G2B
delete from BOOKS
delete from Genre

*/
select * from Genre
select* from BOOKS
select * from G2B
select * from USERS
select * from PersonallyLoadedBooks 