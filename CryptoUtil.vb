Imports System.Diagnostics
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class CryptoUtil

    '8 bytes randomly selected for both the Key and the Initialization Vector
    'the IV is used to encrypt the first block of text so that any repetitive 
    'patterns are not apparent
    Private Shared KEY_64() As Byte = {42, 16, 93, 156, 78, 4, 218, 32}
    Private Shared IV_64() As Byte = {86, 127, 2, 77, 3, 42, 99, 10}

    'Standard DES encryption
    Public Shared Function Encrypt(ByVal value As String) As String

        Try
            If value <> "" Then
                Dim cryptoProvider As DESCryptoServiceProvider = _
                    New DESCryptoServiceProvider()
                Dim ms As MemoryStream = New MemoryStream()
                Dim cs As CryptoStream = _
                    New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), _
                        CryptoStreamMode.Write)
                Dim sw As StreamWriter = New StreamWriter(cs)
                sw.Write(value)
                sw.Flush()
                cs.FlushFinalBlock()
                ms.Flush()

                'convert back to a string
                Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
            End If
        Catch ex As Exception

        End Try
    End Function


    'Standard DES decryption
    Public Shared Function Decrypt(ByVal value As String) As String
        Decrypt = ""
        If value <> "" Then
            Dim cryptoProvider As DESCryptoServiceProvider = _
                New DESCryptoServiceProvider()

            'convert from string to byte array
            Dim buffer As Byte() = Convert.FromBase64String(value)
            Dim ms As MemoryStream = New MemoryStream(buffer)
            Dim cs As CryptoStream = _
                New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), _
                    CryptoStreamMode.Read)
            Dim sr As StreamReader = New StreamReader(cs)

            Return Convert.ToString(sr.ReadToEnd())
        End If
    End Function
End Class
