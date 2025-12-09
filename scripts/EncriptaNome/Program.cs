// Script para gerar valores criptografados
// Uso: dotnet run

using System;
using System.Text;

class Program
{
    private const string DefaultKey = "A1F5E8D9";
    private const int PaddingLength = 25;
    private const char PaddingChar = '+';

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        Console.WriteLine("=== GERANDO VALORES PARA USUARIO ADMIN ===");
        Console.WriteLine();
        
        // Valores a criptografar
        string nome = "admin";
        string senha = "conectairrig@";
        string grupo = "PROGRAMADOR";
        
        string nomeCripto = Encripta(nome);
        string senhaCripto = Encripta(senha);
        string grupoCripto = Encripta(grupo);
        
        Console.WriteLine($"Nome: {nome}");
        Console.WriteLine($"Nome Criptografado: {nomeCripto}");
        Console.WriteLine();
        Console.WriteLine($"Senha: {senha}");
        Console.WriteLine($"Senha Criptografada: {senhaCripto}");
        Console.WriteLine();
        Console.WriteLine($"Grupo: {grupo}");
        Console.WriteLine($"Grupo Criptografado: {grupoCripto}");
        Console.WriteLine();
        Console.WriteLine("=== SQL PARA INSERIR ===");
        Console.WriteLine($"DECLARE @NomeCripto VARCHAR(100) = '{nomeCripto}';");
        Console.WriteLine($"DECLARE @SenhaCripto VARCHAR(100) = '{senhaCripto}';");
        Console.WriteLine($"DECLARE @GrupoCripto VARCHAR(100) = '{grupoCripto}';");
    }

    static string Encripta(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        try
        {
            string encrypted = EncriptaPW(plainText);
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(encrypted);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        catch
        {
            return string.Empty;
        }
    }

    static string EncriptaPW(string text)
    {
        string padded = RPad(text, PaddingLength, PaddingChar);
        string encrypted = Cript(padded, DefaultKey);
        encrypted = encrypted.TrimEnd(PaddingChar);
        return encrypted;
    }

    static string RPad(string text, int length, char paddingChar)
    {
        if (string.IsNullOrEmpty(text))
            return new string(paddingChar, length);
        if (text.Length >= length)
            return text.Substring(0, length);
        return text + new string(paddingChar, length - text.Length);
    }

    static string Cript(string text, string password)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(password))
            return text;

        StringBuilder result = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            char k = password[i % password.Length];
            result.Append((char)(c ^ k));
        }
        return result.ToString();
    }
}
