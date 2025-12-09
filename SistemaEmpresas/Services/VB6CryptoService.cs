using System;
using System.Text;

namespace SistemaEmpresas.Services
{
    /// <summary>
    /// Serviço de criptografia compatível com VB6
    /// Replica o algoritmo de Cript/Encripta/Decripta do sistema legado
    /// </summary>
    public class VB6CryptoService
    {
        private const string DefaultKey = "A1F5E8D9"; // Chave padrão (vgCriptChv do IRRIG.RC linha 1026)
        private const int PaddingLength = 25;
        private const char PaddingChar = '+';

        /// <summary>
        /// Descriptografa uma senha codificada em Base64 usando o algoritmo VB6
        /// </summary>
        public static string Decripta(string encodedText)
        {
            if (string.IsNullOrEmpty(encodedText))
                return string.Empty;

            try
            {
                // Registrar o provider de encoding para suportar Windows-1252
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                
                // 1. Decodifica Base64
                byte[] base64Bytes = Convert.FromBase64String(encodedText);
                
                // 2. Usa Windows-1252 (ANSI) que é o encoding padrão do VB6
                string decodedText = Encoding.GetEncoding(1252).GetString(base64Bytes);

                // 3. Aplica EncriptaPW (que na verdade descriptografa porque XOR é reversível)
                string decrypted = EncriptaPW(decodedText);

                return decrypted;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encripta uma senha usando o algoritmo VB6
        /// </summary>
        public static string Encripta(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                // Registrar o provider de encoding para suportar Windows-1252
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                
                // 1. Aplica EncriptaPW
                string encrypted = EncriptaPW(plainText);

                // 2. Usa Windows-1252 (ANSI) que é o encoding padrão do VB6
                byte[] bytes = Encoding.GetEncoding(1252).GetBytes(encrypted);
                string base64 = Convert.ToBase64String(bytes);

                return base64;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Função EncriptaPW do VB6
        /// x$ = Trim$(Cript$(RPad$(vgSt$, 25, "+"), vgCriptChv))
        /// Remove '+' do final
        /// </summary>
        private static string EncriptaPW(string text)
        {
            // 1. RPad (preenche à direita com '+' até 25 caracteres)
            string padded = RPad(text, PaddingLength, PaddingChar);

            // 2. Aplica Cript
            string encrypted = Cript(padded, DefaultKey);

            // 3. Trim e remove '+' do final
            encrypted = encrypted.TrimEnd(PaddingChar);

            return encrypted;
        }

        /// <summary>
        /// Função Cript do VB6 - XOR com senha
        /// Public Function Cript(St As String, Pw As String) As String
        /// </summary>
        private static string Cript(string text, string password)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(password))
                return text;

            StringBuilder result = new StringBuilder();
            int p = 0; // Ponteiro da senha

            for (int i = 0; i < text.Length; i++)
            {
                // Incrementa ponteiro e reseta se necessário
                p++;
                if (p > password.Length)
                    p = 1;

                // j = Asc(Mid$(Pw$, p, 1)) Or 128
                int j = (int)password[p - 1] | 128;

                // n = Asc(Mid$(St$, i))
                int n = (int)text[i];

                // Encripta com XOR
                bool needsReprocess = true;
                while (needsReprocess)
                {
                    n = n ^ j;

                    // Se char de controle (< 31)
                    if (n < 31)
                    {
                        n = 128 + n;
                        // GoTo DeNovo (continua no loop)
                    }
                    // Se na faixa 128-158 (pode ser char de controle)
                    else if (n > 127 && n < 159)
                    {
                        n = n - 128;
                        // GoTo DeNovo (continua no loop)
                    }
                    else
                    {
                        needsReprocess = false;
                    }
                }

                result.Append((char)n);
            }

            return result.ToString();
        }

        /// <summary>
        /// Função RPad do VB6 - Preenche string à direita
        /// </summary>
        private static string RPad(string text, int length, char padChar)
        {
            if (text.Length >= length)
                return text;

            return text.PadRight(length, padChar);
        }

        /// <summary>
        /// Valida se uma senha em texto plano corresponde à senha criptografada
        /// </summary>
        public static bool ValidatePassword(string plainPassword, string encryptedPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(encryptedPassword))
                return false;

            try
            {
                // Descriptografa a senha do banco
                string decrypted = Decripta(encryptedPassword);

                // Remove padding do final
                decrypted = decrypted.TrimEnd(PaddingChar);

                // Compara (case-insensitive como o VB6)
                return string.Equals(plainPassword, decrypted, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
