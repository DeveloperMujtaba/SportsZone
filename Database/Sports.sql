create table users
(
userid int primary key identity(1,1) not null,
username varchar(100) unique not null, -- coach-club-player
email nvarchar(50) unique not null,
phone nvarchar(20) unique default '',
passwd nvarchar(500) not null,
usertype varchar(100) not null, -- Super Admin -- Club -- Coach -- Player
_date datetime not null,
-- verification
isemail bit default 0,
isphone bit default 0,
_status bit default 1,
ispayment bit default 0,
);
go
create table games(
gameid int primary key identity(1,1) not null,
gamename nvarchar(100) unique not null,
other nvarchar(1000) default ''
);
go
create table clubs
(
clubid int primary key not null,
userid int unique not null,
constraint "club associated with user" foreign key(userid) references users(userid),
clubname nvarchar(500) default '',
city nvarchar(100) default '',
_state varchar(100) default '',
_address nvarchar(500) default '',
lat nvarchar(500) default '',
long nvarchar(500) default '',
logo nvarchar(500) default '',
cover nvarchar(500) default '',
);
go
create table club_games
(
id int primary key identity(1,1) not null,
clubid int not null,
constraint "which club in club games" foreign key(clubid) references clubs(clubid),
gameid int not null,
constraint "which game in club games" foreign key(gameid) references games(gameid),
);
go
create table games_positions
(
positionid int primary key identity(1,1) not null,
gameid int not null,
constraint "game positions in game_position" foreign key(gameid) references games(gameid),
position nvarchar(100) unique not null,
);
go
create table coachs
(
coachid int primary key identity(1,1) not null,
userid int unique not null,
constraint "coach associated with user" foreign key(userid) references users(userid),  
name nvarchar(100) default '',
age int default 0,
picture nvarchar(500) default '',
cover nvarchar(500) default '',
positionid int not null,
constraint "coach type" foreign key(positionid) references games_positions(positionid),
bio nvarchar(500) default '',
);
go
create table teams
(
teamid int primary key identity(1,1) not null,
clubid int not null,
constraint "team of which club" foreign key(clubid) references clubs(clubid),
gameid int not null,
constraint "team of which game" foreign key(gameid) references games(gameid),
name nvarchar(100) unique not null,
logo nvarchar(500) default '',
cover nvarchar(500) default ''
);
go
create table players
(
playerid int primary key identity(1,1) not null,
userid int unique not null,
constraint "player user rel" foreign key (userid) references users(userid),
playername nvarchar(100) default '',
age int default 0,
photo nvarchar(500) default '',
cover nvarchar(500) default '',
height float default 5.5,
bio nvarchar(1000) default '',
roleid int not null,
constraint "user role in game" foreign key(roleid) references games_positions(positionid),
);
go
create table player_associations
(
paid int primary key identity(1,1) not null,
playerid int not null,
clubid int not null,
teamid int not null,
roleid int not null,
_date datetime not null,
constraint "its gives the player id " foreign key(playerid) references players(playerid),
constraint "a player associate in which club" foreign key(clubid) references clubs(clubid),
constraint "a player associate in which team" foreign key(teamid) references teams(teamid),
constraint "a player is playing which role of game" foreign key(roleid) references player_role(roleid),
);
go
create table coach_associations
(
cid int primary key identity(1,1) not null,
coachid int not null,
clubid int not null,
teamid int not null,
positionid int not null,
_date datetime not null,
constraint "its gives the coach id" foreign key(coachid) references coachs(coachid),
constraint "coach associate in which club" foreign key(clubid) references clubs(clubid),
constraint "coach associate in which team" foreign key(teamid) references teams(teamid),
constraint "coach is playing which role of game" foreign key(positionid) references games_positions(positionid),
);
go
create table player_associations_request
(
parid int primary key identity(1,1) not null,
playerid int not null,
clubid int not null,
teamid int not null,
roleid int not null,
_date datetime not null,
constraint "its shows player request id " foreign key(playerid) references players(playerid),
constraint "its shows request to club  " foreign key(clubid) references clubs(clubid),
constraint "its shows request of teams" foreign key(teamid) references teams(teamid),
constraint "its shows request of which role base" foreign key(roleid) references player_role(roleid),
parstatus bit default 0
);
go
create table coach_associations_request
(
carid int primary key identity(1,1) not null,
coachid int not null,
clubid int not null,
teamid int not null,
positionid int not null,
_date datetime not null,
constraint "its shows coach request id of coach" foreign key(coachid) references coachs(coachid),
constraint "its shows request to club of coach" foreign key(clubid) references clubs(clubid),
constraint "its shows request of teams of coach" foreign key(teamid) references teams(teamid),
constraint "its shows request of coach of which base" foreign key(positionid) references games_positions(positionid),
carstatus bit default 0
);
go
create table feedback
(
id int primary key identity(1,1) not null,
_to int not null,
_from int not null,
rating int not null,
msg nvarchar(500),
_date datetime not null,
constraint "feedback to which user" foreign key (_to) references users(userid),
constraint "feedback from which user" foreign key (_from) references users(userid),
);

-- go
go
create table matches
(
mid int primary key identity(1,1) not null,
teamid1 int not null,
teamid2 int not null,
loc nvarchar(500) not null,
_date datetime not null
);
go
create table match_result
(
rid int primary key identity(1,1) not null,
mid int not null,
teamid int not null,
_status char default 'L', --Lose=L, Draw=D, Win=W 
points int not null,
constraint "which match which team" foreign key (mid) references matches(mid),
constraint "loser and winner" foreign key(teamid) references teams(teamid),
);

insert into games values('Cricket','')
insert into games values('Football','')
insert into games values('Hockey','')
insert into games_positions values(1,'WicketKeeper')
insert into games_positions values(1,'Batsman')
insert into games_positions values(1,'Bowler')
insert into games_positions values(3,'Goalkeeper')
insert into games_positions values(3,'Full-Backs')
insert into games_positions values(3,'Centre-Backs')
insert into games_positions values(4,'Forwards')
insert into games_positions values(4,'Defense')
insert into games_positions values(4,'Goaltender/Goalie')
select * from games_positions
select * from users
select * from players
insert into players values(11, 'Mujtaba Khan', 23, '', '', 5.8, 'This is mujtaba, batsman', 2)
select * from games_positions where positionid=2

update players set photo='no-image.jpg'
update players set cover='no-cover.jpg'

select * from coachs
select * from games
select * from games_positions
select * from clubs
update clubs set
clubname='Test Champions Club',
city='Mailsi',
_state='Punjab',
_address='Mailsi Vehari Road Ahmad Pur',
lat='29.8566495',
long='72.2643113',
logo='no-image.jpg',
cover='no-cover.jpg'