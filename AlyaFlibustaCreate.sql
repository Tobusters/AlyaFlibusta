create table Genre(
Id int identity(1, 1) primary key,
GenreName varchar(50) not null,
)
create table Author(
Id int identity(1, 1) primary key,
FirstName varchar(50) not null,
LastName varchar(50) not null,
)

CREATE TABLE BOOKS(   
Id int identity(1, 1) primary key,
  BookName nvarchar(100) not null,
  AuthorId int references Author(Id) not null,
  BookDescription varchar(1000) not null,
  DateOfUpload datetime not null,
  Filepath varchar(200) not null,
  BookIMG varchar(200)
  );

  create table G2B(
  BookID int references BOOKS(Id)not null,
  GenreID int references Genre(Id)not null,
  )

  CREATE TABLE USERS
  (  Id int identity(1, 1) primary key,
  IsBanned bit not null default 0 check(IsBanned >=0 and IsBanned <= 1),
  UserStatus bit not null default 0 check(UserStatus >=0 and UserStatus <= 2),
  Login nvarchar(100) not null unique,
  Email varchar(100) not null,
  Password varchar(256) not null,
  NickName nvarchar(50),
  AvatarIMG varchar(200)
  );
CREATE TABLE COMMENTS (
  Id int identity(1, 1) primary key,
  UserID int references USERS(Id)not null,
  BookID int references BOOKS(Id)not null,
  WrittenDate date not null, 
  TextOf nvarchar(1000)not null 
  );
  CREATE TABLE Estimation (
  UserId int references USERS(id) not null,
  BookID int references BOOKS(id) not null, 
  Est float not null,
  );
  CREATE TABLE PersonalLibrary (
  UserID int references USERS(Id) not null,
  BookID int references BOOKS(id) not null,
);
CREATE TABLE PersonallyLoadedBooks( 
  UserID int references USERS(Id)not null,
  BookID int references BOOKS(id) not null,
);