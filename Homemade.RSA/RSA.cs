using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Homemade.RSA
{
    internal class RSA
    {
        private readonly BigInteger[] ferma = {3,5,17,257,65537};
        public void test()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string pathIn = @"Input.txt";
            if (!File.Exists(pathIn))
            {
                File.Create(pathIn).Close();
                File.WriteAllText(pathIn, "test how to work");
            }
            byte[] _m = File.ReadAllBytes(pathIn);
            //текст шифруеться в байти utf-8 там уже он ложиться под модуль 
            Random rnd = new Random();
            Console.WriteLine("dasd");
            BigInteger p = RandomBigIntegerPrimeFill(256);
            BigInteger q = RandomBigIntegerPrimeFill(256);
            BigInteger n = p * q;
            BigInteger phi = (p-1)*(q-1);
            Console.WriteLine(p);
            Console.WriteLine("");
            Console.WriteLine(n);
            List<BigInteger> fermaPass = new List<BigInteger>();
            for (int i = 0;i < 5; i++)
            {
                if (EuclidAlgorytm(ferma[i],phi) == 1)
                {
                    fermaPass.Add(ferma[i]);
                }
            }
            BigInteger x = 0;
            BigInteger y = 0;
            BigInteger e = fermaPass[rnd.Next(0,fermaPass.Count()-1)];
            Console.WriteLine("e:" + e);
            Console.WriteLine("phi" + phi);
            gcdExtended(e, phi, ref x, ref y);
            // текст на блоки не больше длиной ключа e или n так и не понял 
            BigInteger d = x;
            if (d < 0)
            {
                d += phi;
            }
            BigInteger m = new BigInteger(_m);
            Console.WriteLine("собщения :" + m);
            BigInteger encrypt = BigInteger.ModPow(m,e,n);
            Encrypt(_m, e, n);
            Console.WriteLine("encrypt :" + encrypt);
            BigInteger decrtypt = BigInteger.ModPow(encrypt, d, n);
            Console.WriteLine("Розшифр :" + decrtypt);
            string test = Decrypt(d,n);
            //test block RSA
            //publick (e,n)
            //privatik (d,n)
            Console.WriteLine("end");
        }
        public void Encrypt(byte[] text,BigInteger e, BigInteger n)
        {
            if (!File.Exists("Encrypt.txt"))
            {
                File.Create("Encrypt.txt").Close();
            }
            using (StreamWriter sw = new StreamWriter("Encrypt.txt"))
            {
                BigInteger salt = 320;
                foreach (var b in text)
                {
                    salt = (b + salt + n) % n;
                    Console.WriteLine("Записался байт" + salt + "А был :" + b);
                    sw.WriteLine(BigInteger.ModPow(salt, e, n));
                }
            }
        }
        public string Decrypt(BigInteger d, BigInteger n)
        {
            if (!File.Exists("decrypt.txt"))
            {
                File.Create("decrypt.txt").Close();
            }
            using (StreamReader sr = new StreamReader("Encrypt.txt"))
            {
                int blockCount = File.ReadLines("Encrypt.txt").Count();
                byte[] text = new byte[blockCount];
                //BigInteger temp = 255;
                List <BigInteger> list = new List <BigInteger>();
                for (int i = 0; i < blockCount; i++)
                {
                    BigInteger bigInteger = BigInteger.ModPow(BigInteger.Parse(sr.ReadLine()),d,n);
                    //temp = (temp - bigInteger) % n;
                    //text[i] = byte.Parse(bigInteger.ToString());
                    list.Add(bigInteger);
                }
                BigInteger salt = 320;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        salt = (list[i] - salt) % n;
                    }
                    else
                    {
                        salt = (list[i] - list[i-1]) % n;
                    }
                    text[i] = byte.Parse((salt).ToString());
                    Console.WriteLine("выход :" + (salt));
                }

                File.WriteAllBytes("decrypt.txt",text);
            }
            return "";
        }
        private BigInteger RandomBigIntegerPrimeFill(int bits)
        {
            Random random = new Random();
            int byteSize = (bits * 7) / 8;
            byte[] data = new byte[byteSize];
            while (true)
            {
                random.NextBytes(data);
                BigInteger number = new BigInteger(data);
                if (number > 0 )
                {
                    if(TestMillerRabin(number,10) == true)
                    {
                        break;
                    }
                }
            }
            return new BigInteger(data);
        }
        public BigInteger gcdExtended(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            // Base Case
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            // To store results of
            // recursive call
            BigInteger x1 = 1, y1 = 1;
            BigInteger gcd = gcdExtended(b % a, a, ref x1, ref y1);
            // Update x and y using
            // results of recursive call
            x = y1 - (b / a) * x1;
            y = x1;
            return gcd;
        }
        private BigInteger EuclidAlgorytm(BigInteger r1, BigInteger r2)
        {

            while (r2 != 0)
            {
                BigInteger temp = r2;
                r2 = r1 % r2;
                r1 = temp;
            }
            return r1;
        }
            private bool TestMillerRabin(BigInteger n,int k)
        {
            if (n == 2 || n == 3)
                return true;    
            if(n < 2 || n % 2 == 0)
                return false;

            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s+=1;
            }
            for (int i = 0; i < k; i++)
            {
                Random rnd = new Random();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                   rnd.NextBytes(_a);
                   a = new BigInteger(_a);
                } while (a < 2 || a >= n-2 );

                BigInteger x = BigInteger.ModPow(a, t, n);
                if(x == 1 || x == n - 1)
                {
                    continue;
                }
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if(x == 1)
                    {
                        return false;
                    }
                    if(x == n-1)
                    {
                        break;
                    }
                }
                if(x != n-1)
                {
                    return false;
                }
                
            }
            return true;

        }
    }
    
}
