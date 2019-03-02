using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseProjeLinkedin
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SqlConnection baglan = new SqlConnection(@"Data Source=DESKTOP-UOKUQI6\SQLEXPRESS;Initial Catalog=LinkedInDatabase;Integrated Security=True");
            baglan.Open();
            
            SqlCommand tableCreator = new SqlCommand(
                "DROP TABLE RECOMMENDATION, CONNECTION, JOINED_GROUP, FOLLOWING_ORGANIZATION," +
                "REF_PROFILE_SECTION, PROFILE_SECTION, REF_CV_SECTION, CV_SECTION, PROFILE, CV, GROUPS, POST," +
                "PERSON, ORGANIZATION, USERS, ADDRESS; " +
                "DROP FUNCTION myFunction, checkGroupDate, checkPasswordLenght; " +
                "" +
                "" +
                "CREATE TABLE ADDRESS(" +
                "Address_ID int Identity(1,1) NOT NULL," +
                "Line varchar(100) NULL," +
                "City varchar(20) NULL," +
                "State_County_Province varchar(20) NULL," +
                "Zip_or_Post_Code varchar(10) NULL," +
                "Country varchar(20) NULL," +
                "PRIMARY KEY( Address_ID ));" +
                "" +
                "" +
                "CREATE TABLE USERS(" +
                "User_ID int Identity(1,1) NOT NULL," +
                "Address_ID int NULL," +
                "PRIMARY KEY( User_ID )," +
                "FOREIGN KEY( Address_ID ) REFERENCES ADDRESS( Address_ID ) ON UPDATE CASCADE ON DELETE SET NULL);" +
                "" +
                "" +
                "CREATE TABLE ORGANIZATION(" +
                "Organization_ID int Identity(1,1) NOT NULL," +
                "Organization_Name varchar(50) NOT NULL," +
                "Organization_Description varchar(200) NULL," +
                "User_ID int NOT NULL," +
                "PRIMARY KEY( Organization_ID )," +
                "FOREIGN KEY( User_ID ) REFERENCES USERS( User_ID ) ON UPDATE CASCADE ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE PERSON (" +
                "Person_ID int Identity(1,1) NOT NULL," +
                "Marital_Status varchar(15) NULL CHECK (Marital_Status IN ('Bekar','bekar','Evli','evli'))," +
                "Date_Joined date NOT NULL," +
                "Date_Of_Birth date NULL," +
                "Email_Address varchar(50) NOT NULL," +
                "Password varchar(20) NOT NULL," +
                "First_Name varchar(30) NOT NULL," +
                "Last_Name varchar(30) NOT NULL," +
                "Gender varchar(10) NULL, CHECK (Gender IN ('Erkek','erkek','Kadın','kadın'))," +
                "Current_Organization_ID int NULL," +
                "User_ID int NOT NULL," +
                "PRIMARY KEY( Person_ID )," +
                "FOREIGN KEY( Current_Organization_ID ) REFERENCES ORGANIZATION( Organization_ID ) ON UPDATE CASCADE ON DELETE SET NULL," +
                "FOREIGN KEY(User_ID) REFERENCES USERS( User_ID )," +
                "CHECK (Date_Of_Birth < Date_Joined)); " +
                "" +
                "" +
                "CREATE TABLE POST(" +
                "Post_ID int Identity(1,1) NOT NULL," +
                "Content varchar(150) NULL," +
                "Date_Created date NOT NULL," +
                "User_ID int NOT NULL," +
                "PRIMARY KEY( Post_ID )," + 
                "FOREIGN KEY(User_ID) REFERENCES USERS(User_ID) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE GROUPS(" +
                "Group_ID int Identity(1,1) NOT NULL," +
                "Group_Name varchar(50) NOT NULL," +
                "Group_Description varchar(300) NULL," +
                "Group_Date_Started date NOT NULL," +
                "Group_Date_Ended date NULL," +
                "Last_Activity_Date date NULL," +
                "Created_by_Person_ID int NULL," +
                "PRIMARY KEY( Group_ID )," +
                "FOREIGN KEY( Created_by_Person_ID ) REFERENCES PERSON(Person_ID) ON DELETE SET NULL," +
                "CHECK (Group_Date_Started <= Group_Date_Ended AND Group_Date_Started <= Last_Activity_Date)); " +
                "" +
                "" +
                "CREATE TABLE CV(" +
                "CV_ID int Identity(1,1) NOT NULL," +
                "Date_Created date NOT NULL," +
                "Date_Updated date NOT NULL," +
                "Person_ID int NOT NULL," +
                "PRIMARY KEY( CV_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON(Person_ID) ON DELETE CASCADE," +
                "CHECK (Date_Created <= Date_Updated));" +
                "" +
                "" +
                "CREATE TABLE PROFILE(" +
                "Profile_ID int Identity(1,1) NOT NULL," +
                "Date_Created date NOT NULL," +
                "Date_Updated date NOT NULL," +
                "Person_ID int NOT NULL," +
                "PRIMARY KEY( Profile_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON(Person_ID) ON DELETE CASCADE," +
                "CHECK (Date_Created <= Date_Updated));" +
                "" +
                "" +
                "CREATE TABLE CV_SECTION(" +
                "CV_Section_ID int Identity(1,1) NOT NULL," +
                "CV_Section_Name varchar(30) NULL," +
                "Date_Created date NOT NULL," +
                "Date_Updated date NOT NULL," +
                "CV_ID int NOT NULL," +
                "PRIMARY KEY( CV_Section_ID )," +
                "FOREIGN KEY( CV_ID ) REFERENCES CV(CV_ID) ON DELETE CASCADE," +
                "CHECK (Date_Created <= Date_Updated));" +
                "" +
                "" +
                "CREATE TABLE REF_CV_SECTION(" +
                "Ref_CV_Section_ID int Identity(1,1) NOT NULL," +
                "CV_Section_Description varchar(300) NULL," +
                "CV_Section_ID int NOT NULL," +
                "PRIMARY KEY( Ref_CV_Section_ID )," +
                "FOREIGN KEY( CV_Section_ID ) REFERENCES CV_SECTION( CV_Section_ID ) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE PROFILE_SECTION(" +
                "Profile_Section_ID int Identity(1,1) NOT NULL," +
                "Profile_Section_Name varchar(30) NULL," +
                "Date_Created date NOT NULL," +
                "Date_Updated date NOT NULL," +
                "Profile_ID int NOT NULL," +
                "PRIMARY KEY( Profile_Section_ID )," +
                "FOREIGN KEY( Profile_ID ) REFERENCES PROFILE( Profile_ID ) ON DELETE CASCADE," +
                "CHECK (Date_Created <= Date_Updated));" +
                "" +
                "" +
                "CREATE TABLE REF_PROFILE_SECTION(" +
                "Ref_Profile_Section_ID int Identity(1,1) NOT NULL," +
                "Profile_Section_Description varchar(300) NULL," +
                "Profile_Section_ID int NOT NULL," +
                "PRIMARY KEY( Ref_Profile_Section_ID )," +
                "FOREIGN KEY( Profile_Section_ID ) REFERENCES PROFILE_SECTION( Profile_Section_ID ) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE FOLLOWING_ORGANIZATION(" +
                "Person_ID int NOT NULL," +
                "Organization_ID int NOT NULL," +
                "Following_Date date NOT NULL," +
                "PRIMARY KEY( Person_ID,Organization_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON( Person_ID )," +
                "FOREIGN KEY( Organization_ID ) REFERENCES ORGANIZATION( Organization_ID ) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE JOINED_GROUP(" +
                "Person_ID int NOT NULL," +
                "Group_ID int NOT NULL," +
                "Joining_Date date NOT NULL," +
                "PRIMARY KEY( Person_ID,Group_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON( Person_ID )," +
                "FOREIGN KEY( Group_ID ) REFERENCES GROUPS( Group_ID ) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE CONNECTION(" +
                "Connected_Person_ID int NOT NULL," +
                "Person_ID int NOT NULL," +
                "Connection_Started_Date date NOT NULL," +
                "PRIMARY KEY( Connected_Person_ID,Person_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON( Person_ID )," +
                "FOREIGN KEY( Connected_Person_ID ) REFERENCES PERSON( Person_ID ) ON DELETE CASCADE);" +
                "" +
                "" +
                "CREATE TABLE RECOMMENDATION(" +
                "Recommended_Person_ID int NOT NULL," +
                "Person_ID int NOT NULL," +
                "PRIMARY KEY( Recommended_Person_ID,Person_ID )," +
                "FOREIGN KEY( Person_ID ) REFERENCES PERSON( Person_ID )," +
                "FOREIGN KEY( Recommended_Person_ID ) REFERENCES PERSON( Person_ID ) ON DELETE CASCADE);" +
                "");


            tableCreator.Connection = baglan;
            tableCreator.ExecuteNonQuery();

            SqlCommand fonksiyonKomut = new SqlCommand("CREATE FUNCTION myFunction ( " +
                "@Group_Started_Date date) " +
                "RETURNS VARCHAR(5) " +
                "AS " +
                "BEGIN " +
                "IF @Group_Started_Date > (SELECT Date_Joined FROM PERSON,GROUPS WHERE Created_by_Person_ID = Person_ID AND @Group_Started_Date = Group_Date_Started) " +
                "return 'True';" +
                "return 'False' " +
                "END; " +
                "" +
                "" +            
                "",baglan);
            fonksiyonKomut.ExecuteNonQuery();

            SqlCommand fonksiyonKomut2 = new SqlCommand("CREATE FUNCTION checkGroupDate ( " +
                "@joining_date date ) " +
                "RETURNS VARCHAR(5) " +
                "AS " +
                "BEGIN " +
                "IF @joining_date > (SELECT Group_Date_Started FROM GROUPS,JOINED_GROUP WHERE GROUPS.Group_ID = JOINED_GROUP.Group_ID AND @joining_date = Joining_Date) " +
                "return 'True'; " +
                "return 'False' " +
                "END; ", baglan);
            fonksiyonKomut2.ExecuteNonQuery();

            SqlCommand fonksiyonKomut3 = new SqlCommand("CREATE FUNCTION checkPasswordLenght ( " +
                "@password VARCHAR(20)) " +
                "RETURNS VARCHAR(5) " +
                "AS " +
                "BEGIN " +
                "IF LEN(@password) > 5 " +
                "return 'True'; " +
                "return 'False' " +
                "END; ",baglan);
            fonksiyonKomut3.ExecuteNonQuery();

            SqlCommand alterGroup = new SqlCommand("ALTER TABLE GROUPS " +
                "ADD CONSTRAINT kontrol CHECK(dbo.myFunction(GROUPS.Group_Date_Started) = 'True');" +
                "" +
                "" +
                "ALTER TABLE JOINED_GROUP " +
                "ADD CONSTRAINT kontrol2 CHECK(dbo.checkGroupDate(JOINED_GROUP.Joining_Date) = 'True');" +
                "" +
                "" +
                "ALTER TABLE PERSON " +
                "ADD CONSTRAINT kontrol3 CHECK(dbo.checkPasswordLenght(PERSON.Password) = 'True'); " +
                "", baglan);

            alterGroup.ExecuteNonQuery();


            SqlCommand komut2 = new SqlCommand("CREATE TRIGGER DELETEUSERS " +
                "ON USERS " +
                "INSTEAD OF DELETE " +
                "AS " +
                "BEGIN " +
                "DELETE FROM PERSON WHERE User_ID IN (SELECT User_ID FROM DELETED) " +
                "DELETE FROM USERS WHERE User_ID IN (SELECT User_ID FROM DELETED) " +
                "END; " +
                "" +
                "" +
                "", baglan);
            komut2.ExecuteNonQuery();

            SqlCommand komut3 = new SqlCommand("CREATE TRIGGER DELETEPERSON " +
                "ON PERSON " +
                "INSTEAD OF DELETE " +
                "AS " +
                "BEGIN " +
                "DELETE FROM FOLLOWING_ORGANIZATION WHERE Person_ID IN (SELECT Person_ID FROM DELETED) " +
                "DELETE FROM JOINED_GROUP WHERE Person_ID IN(SELECT Person_ID FROM DELETED) " +
                "DELETE FROM CONNECTION WHERE Person_ID IN (SELECT Person_ID FROM DELETED) " +
                "DELETE FROM RECOMMENDATION WHERE Person_ID IN (SELECT Person_ID FROM DELETED) " +
                "DELETE FROM PERSON WHERE Person_ID IN (SELECT Person_ID FROM DELETED) " +
                "END; ", baglan);
            komut3.ExecuteNonQuery();

            SqlCommand komut5 = new SqlCommand("CREATE TRIGGER ADDPROFILE " +
                "ON PERSON " +
                "AFTER INSERT " +
                "AS " +
                "BEGIN " +
                "DECLARE @var1 date, @var2 date, @var3 int " +
                "SELECT @var1 = Date_Joined FROM INSERTED " +
                "SELECT @var2 = Date_Joined FROM INSERTED " +
                "SELECT @var3 = Person_ID FROM INSERTED " +
                "INSERT INTO PROFILE(Date_Created, Date_Updated, Person_ID) " +
                "VALUES(@var1, @var2, @var3) " +
                "END; " +
                "",baglan);
            komut5.ExecuteNonQuery();


        

            

            SqlCommand komut4 = new SqlCommand("" +
                "INSERT INTO ADDRESS(Line, City, State_County_Province, Zip_or_Post_Code, Country) " +
                "VALUES ('4326. sokak', 'Izmir', 'Bornova', '35000', 'Turkiye'), " +
                "('Cesme sokak', 'Kirklareli', 'Luleburgaz', '39000', 'Turkiye'), " +
                "('cikmaz sokak', 'Izmir', 'torbali', '35000', 'Turkiye'), " +
                "('Ramazan mah. Tas sokak', 'Usak', 'Merkez', '64000', 'Turkiye'), " +
                "('4532 sokak', 'Izmir', 'Tepecik', '35000', 'Turkiye')" +
                "" +
                "" +
                "INSERT INTO USERS(Address_ID) " +
                "VALUES (1), (2), (3), (4), (5) " +
                "" +
                "" +
                "INSERT INTO ORGANIZATION(Organization_Name, Organization_Description, User_ID) " +
                "VALUES ('Ceylin Koltuk Doseme', 'Her turlu cekyat-koltuk tamiri yapilir', 2), " +
                "('S-R yapi market', 'Her turlu hirdavat malzemesi', 1)" +
                "" +
                "" +
                "INSERT INTO PERSON(Marital_Status, Date_Joined, Date_of_Birth, Email_Address, Password, " +
                "First_Name, Last_Name, Gender, Current_Organization_ID, User_ID) " +
                "VALUES ('bekar', '2012.01.01', '1998.02.16', 'serdar@serdar.com', '123456', " +
                "'Serdar', 'Dal', 'Erkek', 2, 3) " +
                "" +
                "" +
                "INSERT INTO PERSON(Marital_Status, Date_Joined, Date_of_Birth, Email_Address, Password, " +
                "First_Name, Last_Name, Gender, Current_Organization_ID, User_ID) "  +
                "VALUES ('Bekar', '2013.01.02', '1995.07.06', 'talat@talat.com', '12345678', " +
                "'Talat', 'Ceyhun', 'Erkek', 1, 2) " +
                "" +
                "" +
                "INSERT INTO PERSON(Marital_Status, Date_Joined, Date_of_Birth, Email_Address, Password, " +
                "First_Name, Last_Name, Gender, Current_Organization_ID, User_ID) " +
                "VALUES ('Evli', '2012-12-15', '1973-03-06', 'mesutkayali@hotmail.com', '198237', " +
                " 'Mesut', 'Kayali', 'Erkek', NULL, '5' ) " +
                "" +
                "" +
                "INSERT INTO POST(Content, Date_Created, User_ID) " +
                "VALUES('Bugun hava cok guzel', GETDATE(), 2), " +
                "('Yilan yap kendini tisss', GETDATE(), 3), " +
                "('Bu gece sabaha kadar ödev yaptık.', GETDATE(), 2), " +
                "('Yeni projemiz sizlerle', GETDATE(), 5), " +
                "('En sevdiğim hocam OSMAN hocam', GETDATE(),3), " +
                "('İzmirde en iyi kazandibi Süt Çiçeğinde yenir', GETDATE(),3), " +
                "('Benim eski öğrencim üç katlı integrali kafadan çözerdi', GETDATE(), 5), " +
                "('Aman aman nerelere geldik daha demin evdeydik', GETDATE(), 3) " +
                "" +
                "" +
                "INSERT INTO GROUPS(Group_Name, Group_Description, Group_Date_Started, Group_Date_Ended," +
                "Last_Activity_Date, Created_by_Person_ID) " +
                "VALUES('Ege Bilmuh','Ege Üniversitesi Bilgisayar Mühendisliği Mezunları','2014-12-15', NULL, '2018-12-19', 1), " + //id1
                "('Kepirtepe', 'Kepirtepe Anadolu Ogretmen Lisesi Mezunları', '2014-03-16', NULL, '2018-10-11',2 ), " + //id2
                "('Veritabanı Programcıları', 'Veritabanı programcıları grubu', '2015-07-12', NULL, '2018-12-02',2);" + //id3
                "" +
                "" +
                "INSERT INTO JOINED_GROUP(Person_ID, Group_ID, Joining_Date) " +
                "VALUES(1,1,'2015-12-15'), " +
                "(2,1,'2016-03-12'), " +
                "(2,2,'2017-10-19'), " +
                "(1,3,'2018-01-01'), " +
                "(2,3,'2016-11-11'), " +
                "(3,3,'2016-12-04')" +
                "" +
                "" +
                "INSERT INTO CV(Date_Created, Date_Updated, Person_ID) " +
                "VALUES ('2014-01-01', '2017-01-01', 1), " +
                "('2015-03-03', '2018-03-04', 2)" +
                "" +
                "" +
                //"INSERT INTO PROFILE(Date_Created, Date_Updated, Person_ID) " +
                //"VALUES ('2012-01-01', '2018-07-07', 1), " + // serdar profil id=1
                //"('2013-01-02', '2017-09-04', 2), " + // talat profil id=2
                //"('2012-12-15', '2018-04-12', 3)" + // mesut profil id=3
                "" +
                "UPDATE PROFILE SET Date_Updated = '2018-07-07' WHERE Profile_ID = 1;" +
                "UPDATE PROFILE SET Date_Updated = '2017-09-04' WHERE Profile_ID = 2;" +
                "UPDATE PROFILE SET Date_Updated = '2018-04-12' WHERE Profile_ID = 3;" +
                "" +
                "" +
                "INSERT INTO CV_SECTION(CV_Section_Name, Date_Created, Date_Updated, CV_ID) " +
                "VALUES ('Okullar', '2015-08-08','2018-08-08', 1 ), " +
                "('Bildiği yabancı diller', '2016-10-02', '2017-03-08', 1), " +
                "('Okullar', '2016-09-04', '2018-09-04', 2), " +
                "('Bildiği yabancı diller', '2016-10-02', '2018-02-02', 2)" +
                "" +
                "" +
                "INSERT INTO REF_CV_SECTION(CV_Section_Description, CV_Section_ID) " +
                "VALUES ('Izmir Atatürk Lisesi', 1)," +
                "('Ege Üniversitesi Bilgisayar Mühendisliği', 1), " +
                "('ingilizce', 2), " +
                "('almanca', 2), " +
                "('Kepirtepe AOL', 3), " +
                "('Ege Bilmuh', 3), " +
                "('ingilizce', 4)" +
                "" +
                "" +
                "INSERT INTO PROFILE_SECTION(Profile_Section_Name, Date_Created, Date_Updated, Profile_ID) " +
                "VALUES ('İlgi Alanları', '2015-08-08', '2015-08-08', 1)," +//serdar ilgi alanları
                "('İletişim Bilgileri', '2016-09-04', '2016-09-04', 2)," +// talat iletişim bilgileri
                "('iletişim bilgileri', '2017-04-13','2017-07-21', 1), " +//serdar iletişim bilgileri
                "('ilgi alanları', '2017-12-15','2018-03-08', 2)" + // talat ilgi alanları
                "" +
                "" +
                "INSERT INTO REF_PROFILE_SECTION(Profile_Section_Description, Profile_Section_ID) " +
                "VALUES ('05439227459', 2), " +
                "('akvaryum karides', 1), " +
                "('05073094302', 3), " +
                "('bisiklet turculuğu',4)" +
                "" +
                "" +
                "INSERT INTO FOLLOWING_ORGANIZATION(Person_ID, Organization_ID, Following_Date) " +
                "VALUES (2, 1, '2018-01-02'), " +
                "(2, 2, '2017-12-14') " +
                "" +
                "" +
                "INSERT INTO CONNECTION(Connected_Person_ID, Person_ID, Connection_Started_Date) " +
                "VALUES (1, 2, '2016-01-01') " +
                "" +
                "" +
                "INSERT INTO RECOMMENDATION(Person_ID, Recommended_Person_ID) " +
                "VALUES (1, 3), " +
                "(2, 3), " +
                "(3,1), " +
                "(3,2)" +
                "" +
                "", baglan);
            //komut4.ExecuteNonQuery();
            try
            {
                komut4.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Hatalı veri girişi.");
            }




            Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());

            }

        }
    }

