using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class FileDataHandler 
{
    private string dataFileName = string.Empty;
    private string dataDirPath = string.Empty;

    public FileDataHandler(string path, string fileName)
    {
        dataDirPath = path;
        dataFileName = fileName;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            // Generate completely random key/IV for this save
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();

                // Encrypt the data with random key/IV
                byte[] encryptedData = EncryptStringToBytes(dataToStore, aes.Key, aes.IV);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    // Write key, IV, then encrypted data
                    stream.Write(aes.Key, 0, aes.Key.Length);    // 32 bytes
                    stream.Write(aes.IV, 0, aes.IV.Length);      // 16 bytes
                    stream.Write(encryptedData, 0, encryptedData.Length);
                }
            }

            //Debug.Log($"Game data successfully saved with random encryption to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game data: {e}");
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // Read the random key and IV
                    byte[] key = new byte[32]; // AES-256 key size
                    byte[] iv = new byte[16];  // AES IV size

                    stream.Read(key, 0, key.Length);
                    stream.Read(iv, 0, iv.Length);

                    // Read remaining encrypted data
                    byte[] encryptedData = new byte[stream.Length - stream.Position];
                    stream.Read(encryptedData, 0, encryptedData.Length);

                    // Decrypt game data
                    string decryptedData = DecryptStringFromBytes(encryptedData, key, iv);
                    loadedData = JsonUtility.FromJson<GameData>(decryptedData);
                }

                //Debug.Log($"Game data successfully loaded from: {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game data: {e}");
            }
        }

        return loadedData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    #region AES Encryption Methods
    private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));
        if (key == null || key.Length != 32)
            throw new ArgumentException("Key must be 32 bytes for AES-256");
        if (iv == null || iv.Length != 16)
            throw new ArgumentException("IV must be 16 bytes");

        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
    {
        if (cipherText == null || cipherText.Length == 0)
            throw new ArgumentNullException(nameof(cipherText));
        if (key == null || key.Length != 32)
            throw new ArgumentException("Key must be 32 bytes for AES-256");
        if (iv == null || iv.Length != 16)
            throw new ArgumentException("IV must be 16 bytes");

        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
    #endregion
}
