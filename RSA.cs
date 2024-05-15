using System;
using System.Numerics;

public class Program
{
    public static void Main()
    {
        RSA rsa = new RSA();
        bool execute = false;

        while (!execute)
        {
            try
            {
                Console.Write("Enter the first prime number (p): ");
                int p = int.Parse(Console.ReadLine());
                if (!IsPrime(p))
                {
                    throw new Exception("p is not a prime number.");
                }

                Console.Write("Enter the second prime number (q): ");
                int q = int.Parse(Console.ReadLine());
                if (!IsPrime(q))
                {
                    throw new Exception("q is not a prime number.");
                }

                int n = p * q;
                int phi = (p - 1) * (q - 1);

                Console.Write("Enter the public exponent (e): ");
                int e = int.Parse(Console.ReadLine());
                if (GCD(e, phi) != 1)
                {
                    throw new Exception("e is not coprime with φ(n).");
                }

                Console.Write("Do you want to encrypt or decrypt? (e/d): ");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "e")
                {
                    Console.Write("Enter the message to be encrypted (M): ");
                    int M = int.Parse(Console.ReadLine());

                    int encryptedMessage = rsa.Encrypt(p, q, M, e);
                    Console.WriteLine($"Encrypted message: {encryptedMessage}");
                }
                else if (choice.ToLower() == "d")
                {
                    Console.Write("Enter the ciphertext to be decrypted (C): ");
                    int C = int.Parse(Console.ReadLine());

                    int decryptedMessage = rsa.Decrypt(p, q, C, e);
                    Console.WriteLine($"Decrypted message: {decryptedMessage}");
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'e' for encryption or 'd' for decryption.");
                }

                execute = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}

public class RSA
{
    public int Encrypt(int p, int q, int M, int e)
    {
        int n = p * q;
        return ModuloExponentiation(M, e, n);
    }

    private int ModuloExponentiation(int baseValue, int exponent, int modulus)
    {
        BigInteger result = 1;
        BigInteger baseVal = baseValue % modulus;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
            {
                result = (result * baseVal) % modulus;
            }
            exponent >>= 1;
            baseVal = (baseVal * baseVal) % modulus;
        }

        return (int)result;
    }

    private int ModuloInverse(int a, int m)
    {
        int m0 = m, t, q;
        int x0 = 0, x1 = 1;

        if (m == 1) return 0;

        while (a > 1)
        {
            q = a / m;
            t = m;
            m = a % m;
            a = t;
            t = x0;
            x0 = x1 - q * x0;
            x1 = t;
        }

        if (x1 < 0) x1 += m0;
        return x1;
    }

    public int Decrypt(int p, int q, int C, int e)
    {
        int n = p * q;
        int phi = (p - 1) * (q - 1);
        int d = ModuloInverse(e, phi);
        return ModuloExponentiation(C, d, n);
    }
}
