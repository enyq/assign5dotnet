Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Odbc
Imports MySql.Data.MySqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PackageWebService
    Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Function AddPackage(ByVal id As String, ByVal destination As String, ByVal weight As String, ByVal userId As String) As String

        Dim conn As New MySqlConnection
        conn.ConnectionString = "server=localhost; user id=root; password = admin; database = assign5"
        conn.Open()
        Dim query As String = "insert into packages values('{0}','{1}','{2}')"
        query = String.Format(query, id, destination, weight)
        Dim command As New MySqlCommand(query, conn)
        command.ExecuteNonQuery()

        Dim relId As New Random()

        Dim query2 As String = "insert into userpackage values('{0}','{1}','{2}','none')"
        query2 = String.Format(query2, relId.Next, userId, id)
        Dim command2 As New MySqlCommand(query2, conn)
        command2.ExecuteNonQuery()

        conn.Close()
        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Function RemovePackage(ByVal id As String) As String

        Dim conn As New MySqlConnection
        conn.ConnectionString = "server=localhost; user id=root; password = admin; database = assign5"
        conn.Open()

        Dim query3 As String = "select id from userpackage where packageId='{0}'"
        query3 = String.Format(query3, id)
        Dim command3 As New MySqlCommand(query3, conn)

        Dim userpackageId As String

        Dim myread As MySqlDataReader
        myread = command3.ExecuteReader()
        If myread.Read() Then
            userpackageId = myread("id").ToString
        End If

        myread.Close()


        Dim query4 As String = "delete from trackedpackages where userpackageId='{0}'"
        query4 = String.Format(query4, userpackageId)
        Dim command4 As New MySqlCommand(query4, conn)
        command4.ExecuteNonQuery()

        Dim query2 As String = "delete from userpackage where packageId='{0}'"
        query2 = String.Format(query2, id)
        Dim command2 As New MySqlCommand(query2, conn)
        command2.ExecuteNonQuery()

        Dim query As String = "delete from packages where id='{0}'"
        query = String.Format(query, id)
        Dim command As New MySqlCommand(query, conn)
        command.ExecuteNonQuery()

        conn.Close()
        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function RegisterPackage(ByVal packageId As String, ByVal location As String) As String

        Dim conn As New MySqlConnection
        conn.ConnectionString = "server=localhost; user id=root; password = admin; database = assign5"
        conn.Open()
        Dim id As New Random()
        Dim query As String = "insert into trackedpackages values('{0}','{1}','{2}','{3}')"
        Dim currentDate As String = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        query = String.Format(query, id.Next, packageId, location, currentDate)
        Dim command As New MySqlCommand(query, conn)
        command.ExecuteNonQuery()

        Dim query2 As String = "update userpackage set status='arrived' where id='{0}'"
        query2 = String.Format(query2, packageId)
        Dim command2 As New MySqlCommand(query2, conn)
        command2.ExecuteNonQuery()
        conn.Close()
        Return "Success"
    End Function


End Class