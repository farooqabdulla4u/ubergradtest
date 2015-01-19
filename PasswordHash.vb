Imports System.Text
Imports System.Security.Cryptography

''' <summary>
''' Salted password hashing with PBKDF2-SHA1.
''' Author: havoc AT defuse.ca
''' www: http://crackstation.net/hashing-security.htm
''' Compatibility: .NET 3.0 and later.
''' </summary>
Public Class PasswordHash
    ' The following constants may be changed without breaking existing hashes.
    Public Const SALT_BYTE_SIZE As Integer = 24
    Public Const HASH_BYTE_SIZE As Integer = 24
    Public Const PBKDF2_ITERATIONS As Integer = 1000

    Public Const ITERATION_INDEX As Integer = 0
    Public Const SALT_INDEX As Integer = 1
    Public Const PBKDF2_INDEX As Integer = 2

    ''' <summary>
    ''' Creates a salted PBKDF2 hash of the password.
    ''' </summary>
    ''' <param name="password">The password to hash.</param>
    ''' <returns>The hash of the password.</returns>
    Public Shared Function CreateHash(ByVal password As String, ByVal salt1 As String) As String

        ' Generate a random salt
        Dim csprng As New RNGCryptoServiceProvider()
        Dim salt As Byte() = New Byte(SALT_BYTE_SIZE - 1) {}
        Dim saltBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(salt1)
        csprng.GetBytes(saltBytes)

        'Hash the password anxd encode the parameters
        Dim hash As Byte() = PBKDF2(password, saltBytes, PBKDF2_ITERATIONS, HASH_BYTE_SIZE)
        Return PBKDF2_ITERATIONS & ":" & Convert.ToBase64String(saltBytes) & ":" & Convert.ToBase64String(hash)
    End Function

    ''' <summary>
    ''' Validates a password given a hash of the correct one.
    ''' </summary>
    ''' <param name="password">The password to check.</param>
    ''' <param name="correctHash">A hash of the correct password.</param>
    ''' <returns>True if the password is correct. False otherwise.</returns>
    Public Shared Function ValidatePassword(ByVal password As String, ByVal correctHash As String) As Boolean
        ' Extract the parameters from the hash
        Dim delimiter As Char() = {":"c}
        Dim split As String() = correctHash.Split(delimiter)
        Dim iterations As Integer = Int32.Parse(split(ITERATION_INDEX))
        Dim salt As Byte() = Convert.FromBase64String(split(SALT_INDEX))
        Dim hash As Byte() = Convert.FromBase64String(split(PBKDF2_INDEX))

        Dim testHash As Byte() = PBKDF2(password, salt, iterations, hash.Length)
        Return SlowEquals(hash, testHash)
    End Function

    ''' <summary>
    ''' Compares two byte arrays in length-constant time. This comparison
    ''' method is used so that password hashes cannot be extracted from
    ''' on-line systems using a timing attack and then attacked off-line.
    ''' </summary>
    ''' <param name="a">The first byte array.</param>
    ''' <param name="b">The second byte array.</param>
    ''' <returns>True if both byte arrays are equal. False otherwise.</returns>
    Private Shared Function SlowEquals(ByVal a As Byte(), ByVal b As Byte()) As Boolean
        Dim diff As UInteger = CUInt(a.Length) Xor CUInt(b.Length)
        Dim i As Integer = 0
        While i < a.Length AndAlso i < b.Length
            diff = diff Or CUInt(a(i) Xor b(i))
            i += 1
        End While
        Return diff = 0
    End Function

    ''' <summary>
    ''' Computes the PBKDF2-SHA1 hash of a password.
    ''' </summary>
    ''' <param name="password">The password to hash.</param>
    ''' <param name="salt">The salt.</param>
    ''' <param name="iterations">The PBKDF2 iteration count.</param>
    ''' <param name="outputBytes">The length of the hash to generate, in bytes.</param>
    ''' <returns>A hash of the password.</returns>
    Private Shared Function PBKDF2(ByVal password As String, ByVal salt As Byte(), ByVal iterations As Integer, ByVal outputBytes As Integer) As Byte()
        Dim pbkdf2__1 As New Rfc2898DeriveBytes(password, salt)
        pbkdf2__1.IterationCount = iterations
        Return pbkdf2__1.GetBytes(outputBytes)
    End Function
End Class

