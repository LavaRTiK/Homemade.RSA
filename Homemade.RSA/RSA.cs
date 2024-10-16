using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Homemade.RSA
{
    internal class RSA
    {
        public void test()
        {
            BigInteger p = RandomBigIntegerFill(5);
            Console.WriteLine(p);
            Console.WriteLine("end");
        }
        private BigInteger RandomBigIntegerFill(int bits)
        {
            Random random = new Random();
            int byteSize = (bits * 7) / 8;
            byte[] data = new byte[byteSize];
            while (true)
            {
                random.NextBytes(data);
                BigInteger number = new BigInteger(data);
                Console.WriteLine(number);
                if (number > 0)
                {
                    int temp = 0;
                    for (int i = 2;i < number;i++)
                    {
                        if(number % i == 0)
                        {
                            temp++;
                        }
                        if (temp > 1)
                        {
                            break;
                        }
                    }
                    if(temp == 1)
                    {
                        break ;
                    }
                }
            }
            return new BigInteger(data);
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
                s++;
            }
            for (int i = 0; i < k; i++)
            {
                Random rnd = new Random();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rnd.NextBytes(_a);
                   a = new BigInteger( _a );
                } while (a < 2 || n > n-2 );

                BigInteger x = BigInteger.ModPow(a, t, n);
                if(x == 1 || x == n - 1)
                {
                    continue;
                }
                for (int r = 1; r < s; r++)
                {
                    BigInteger
                }
            }

        }
    }
    
}
