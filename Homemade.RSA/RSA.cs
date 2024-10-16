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
                File.Create(pathIn);
            }
            byte[] _m = File.ReadAllBytes(pathIn);
            //текст шифруеться в байти utf-8 там уже он ложиться под модуль 
            Random rnd = new Random();
            Console.WriteLine("dasd");
            BigInteger p = RandomBigIntegerPrimeFill(5);
            BigInteger q = RandomBigIntegerPrimeFill(5);
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
            gcdExtended(e, phi, ref x, ref y);
            BigInteger d = y;
            BigInteger m = new BigInteger(_m);
            Console.WriteLine("собщения :" + m);
            BigInteger encrypt = BigInteger.ModPow(m,e,n);
            //Console.WriteLine(encrypt);
            Console.WriteLine("");
            BigInteger decrtypt = BigInteger.ModPow(encrypt, d, n);
            Console.WriteLine("Розшифр :" + decrtypt);

            //publick (e,n)
            //privatik (d,n)




            Console.WriteLine("end");
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
