using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace DesAlgorithm
{
    class DesSifreleme
    {


        public string dosyadanOku()
        {
            string dosya_yolu = @"PlainText.txt";

            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);

            StreamReader sw = new StreamReader(fs);
            string metin = sw.ReadToEnd();

            fs.Close();

            return metin;

        }


        public byte[] byteDizisineCevir(string message)
        {
            byte[] dizi = Encoding.ASCII.GetBytes(message);
            return dizi;
        }



        private ulong bitGetir(ulong deger, int bitSayisi, int sira)
        {
            deger = deger >> (bitSayisi - sira);
            ulong bir = 1;
            ulong sonuc = deger & bir;
            return sonuc;

        }



        public ulong baytDizisi64BiteCevir(byte[] message)
        {

            ulong sayi = 0;
            for (int i = 0; i < message.Length; i++)
            {
                sayi = sayi | message[i];
                if (i == 7)
                    break;
                sayi = sayi << 8;
            }
            return sayi;


        }



        public byte[] baytlaraCevir(ulong deger)
        {
            byte[] byteDizisi = new byte[8];

            ulong ikiYüzElliBes = 255;
            for (int i = 7; i >= 0; i--)
            {
                byteDizisi[i] = (byte)(deger & ikiYüzElliBes);
                if (i == 0)
                    break;
                deger = deger >> 8;
            }

            return byteDizisi;
        }



        private ulong PC_1(ulong anahtar)
        {

            int[] pc1_table =
            {
                57,49,41,33,25,17,9,
                1,58,50,42,34,26,18,
                10,2,59,51,43,35,27,
                19,11,3,60,52,44,36,
                63,55,47,39,31,23,15,
                7,62,54,46,38,30,27,
                14,6,61,53,45,37,29,
                21,13,5,28,20,12,4
            };

            ulong PCAnahtar = 0;

            for (int i = 0; i < 56; i++)
            {
                PCAnahtar = PCAnahtar | bitGetir(anahtar, 64, pc1_table[i]);
                if (i == 55)
                    break;
                PCAnahtar = PCAnahtar << 1;

            }

            return PCAnahtar;
        }



        private ulong KeyLeft(ulong PCkey)
        {
            ulong KeyLeft = 0;

            for (int i = 1; i < 29; i++)
            {
                KeyLeft = KeyLeft | bitGetir(PCkey, 56, i);
                if (i == 28)
                    break;
                KeyLeft = KeyLeft << 1;
            }
            return KeyLeft;
        }



        private ulong KeyRight(ulong PCkey)
        {
            ulong KeyRight = 0;

            for (int i = 29; i <= 56; i++)
            {
                KeyRight = KeyRight | bitGetir(PCkey, 56, i);
                if (i == 56)
                    break;
                KeyRight = KeyRight << 1;
            }
            return KeyRight;
        }



        private ulong daireselKaydirma(ulong anahtar, int kaydirmaMiktari)
        {
            ulong maske = anahtar >> (28 - kaydirmaMiktari);
            ulong kaydirma = (anahtar << kaydirmaMiktari);
            ulong sagMaske = (1 << 28) - 1;
            ulong sonuc = kaydirma | maske;

            sonuc = sonuc & sagMaske;
            return sonuc;

        }



        private ulong AnahtarBirlestirme(ulong sag, ulong sol)
        {
            ulong Birlesim = 0;
            Birlesim = Birlesim | sol;
            Birlesim = Birlesim << 28;
            Birlesim = Birlesim | sag;

            return Birlesim;

        }



        private ulong PC_2(ulong Key)
        {
            ulong FinalKey = 0;

            int[] pc2_table =
            {
                14,17,11,24,1,5,
                3,28,15,6,21,10,
                23,19,12,4,26,8,
                16,7,27,20,13,2,
                41,52,31,37,47,55,
                30,40,51,45,33,48,
                44,49,39,56,34,53,
                46,42,50,36,29,32,
            };


            for (int j = 0; j < 48; j++)
            {
                FinalKey = FinalKey | bitGetir(Key, 56, pc2_table[j]);
                if (j == 47)
                    break;
                FinalKey = FinalKey << 1;
            }

            return FinalKey;


        }



        private ulong InitialPermutation(ulong message)
        {


            int[] ip_table =
            {
                58,50,42,34,26,18,10,2,
                60,52,44,36,28,20,12,4,
                62,54,46,38,30,22,14,6,
                64,56,48,40,32,24,16,8,
                57,49,41,33,25,17,9,1,
                59,51,43,35,27,19,11,3,
                61,53,45,37,29,21,13,5,
                63,55,47,39,31,23,15,7

            };
            ulong IPmessage = 0;

            for (int i = 0; i < 64; i++)
            {
                IPmessage = IPmessage | bitGetir(message, 64, ip_table[i]);
                if (i == 63)
                    break;
                IPmessage = IPmessage << 1;

            }

            return IPmessage;
        }


        private ulong IPRightMessage(ulong message)
        {
            ulong RightIP = 0;

            for (int i = 33; i <= 64; i++)
            {
                RightIP = RightIP | bitGetir(message, 64, i);
                if (i == 64)
                    break;
                RightIP = RightIP << 1;
            }
            return RightIP;

        }



        private ulong IPLeftMessage(ulong message)
        {
            ulong LeftIP = 0;

            for (int i = 1; i < 33; i++)
            {
                LeftIP = LeftIP | bitGetir(message, 64, i);
                if (i == 32)
                    break;
                LeftIP = LeftIP << 1;
            }
            return LeftIP;

        }



        private ulong E_bitSelection(ulong RightMessage)
        {


            int[] E_table =
            {
                32,1,2,3,4,5,
                4,5,6,7,8,9,
                8,9,10,11,12,13,
                12,13,14,15,16,17,
                16,17,18,19,20,21,
                20,21,22,23,24,25,
                24,25,26,27,28,29,
                28,29,30,31,32,1

            };
            ulong Right48 = 0;

            for (int i = 0; i < 48; i++)
            {
                Right48 = Right48 | bitGetir(RightMessage, 32, E_table[i]);
                if (i == 47)
                    break;
                Right48 = Right48 << 1;

            }

            return Right48;
        }



        private ulong XOR(ulong RightMessage, ulong Key)
        {
            ulong XOR = RightMessage ^ Key;
            return XOR;
        }



        private ulong S_Boxes(ulong data)
        {
            byte[,] sbox = new byte[8, 64]
            {
                 {
                    14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7,
                    0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8,
                     4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0,
                    15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13
                 },

                 {
                     15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10,
                     3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5,
                     0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15,
                     13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9
                 },

                 {
                     10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8,
                     13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1,
                     13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7,
                     1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12
                 },

                 {
                     7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15,
                     13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9,
                     10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4,
                     3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14
                 },

                 {
                     2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9,
                     14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6,
                     4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14,
                     11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3
                 },

                 {
                     12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11,
                     10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8,
                     9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6,
                     4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13
                 },

                 {
                     4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1,
                     13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6,
                     1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2,
                     6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12
                 },

                 {
                     13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7,
                     1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2,
                     7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8,
                     2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11
                 }
            };

            byte[] bytes = new byte[8];
            byte[] s_box_data = new byte[8];

            for (int i = 7; i >= 0; i--)
            {
                bytes[i] = (byte)(data & 63);
                if (i == 0)
                    break;
                data = data >> 6;
            }

            for (int i = 0; i < 8; i++)
            {
                ulong row = 0;
                row = row | bitGetir(bytes[i], 6, 1);
                row = row << 1;
                row = row | bitGetir(bytes[i], 6, 6);


                ulong column = bytes[i];
                column = column & 30;
                column = column >> 1;

                s_box_data[i] = sbox[i, (row * 16 + column)];

            }

            ulong message = 0;
            for (int i = 0; i < 8; i++)
            {
                message = message | s_box_data[i];
                if (i == 7)
                    break;
                message = message << 4;
            }
            return message;


        }



        private ulong Permutation(ulong data)
        {
            int[] p_table =
            {
                16,7,20,21,
                29,12,28,17,
                1,15,23,26,
                5,18,31,10,
                2,8,24,14,
                32,27,3,9,
                19,13,30,6,
                22,11,4,25

            };

            ulong PermutationMessage = 0;

            for (int i = 0; i < 32; i++)
            {
                PermutationMessage = PermutationMessage | bitGetir(data, 32, p_table[i]);
                if (i == 31)
                    break;
                PermutationMessage = PermutationMessage << 1;

            }

            return PermutationMessage;

        }



        private ulong MesajBirlestirme(ulong Right, ulong Left)
        {
            ulong data = 0;

            data = data | Right;
            data = data << 32;
            data = data | Left;

            return data;

        }



        private ulong FinalInitialPermutation(ulong data)
        {
            int[] ip_table =
            {
                40,8,48,16,56,24,64,32,
                39,7,47,15,55,23,63,31,
                38,6,46,14,54,22,62,30,
                37,5,45,13,53,21,61,29,
                36,4,44,12,52,20,60,28,
                35,3,43,11,51,19,59,27,
                34,2,42,10,50,18,58,26,
                33,1,41,9,49,17,57,25
            };

            ulong FinalIPmessage = 0;

            for (int i = 0; i < 64; i++)
            {
                FinalIPmessage = FinalIPmessage | bitGetir(data, 64, ip_table[i]);
                if (i == 63)
                    break;
                FinalIPmessage = FinalIPmessage << 1;

            }

            return FinalIPmessage;


        }


        private ulong BlokSifreleme(ulong mesaj, ulong anahtar)
        {
            // Anahtarları oluşturma

            ulong pc1_anahtar = PC_1(anahtar);
            ulong sagAnahtar = KeyRight(pc1_anahtar);
            ulong solAnahtar = KeyLeft(pc1_anahtar);
            int[] kaydirmaTablosu = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            ulong[] altAnahtarlar = new ulong[16];
            for (int i = 0; i < 16; i++)
            {
                ulong sagKaydirma = daireselKaydirma(sagAnahtar, kaydirmaTablosu[i]);
                ulong solKaydirma = daireselKaydirma(solAnahtar, kaydirmaTablosu[i]);
                ulong birlesim = AnahtarBirlestirme(sagKaydirma, solKaydirma);
                altAnahtarlar[i] = birlesim;
                sagAnahtar = sagKaydirma;
                solAnahtar = solKaydirma;
            }
            ulong[] pc2_anahtarlar = new ulong[16];
            for (int i = 0; i < 16; i++)
            {
                pc2_anahtarlar[i] = PC_2(altAnahtarlar[i]);
            }


            // Mesajı Şifreleme 

            ulong ip = InitialPermutation(mesaj);
            ulong sagMesaj = IPRightMessage(ip);
            ulong solMesaj = IPLeftMessage(ip);
            for (int i = 0; i < 16; i++)
            {

                ulong genisletilenSagMesaj = E_bitSelection(sagMesaj);
                ulong XorMesaj = XOR(genisletilenSagMesaj, pc2_anahtarlar[i]);
                ulong s_box = S_Boxes(XorMesaj);
                ulong p_mesaj = Permutation(s_box);
                ulong yeniSag = XOR(solMesaj, p_mesaj);
                ulong yeniSol = sagMesaj;
                sagMesaj = yeniSag;
                solMesaj = yeniSol;

            }
            ulong mesajBirlestirme = MesajBirlestirme(sagMesaj, solMesaj);
            ulong sifrelenmisMesaj = FinalInitialPermutation(mesajBirlestirme);

            return sifrelenmisMesaj;

        }

        public byte[] BlokTamamlama(byte[] mesaj)
        {
            int elemanSayisi = mesaj.Length;
            int sonKarakterSayisi = elemanSayisi % 8;

            if (sonKarakterSayisi != 0)
            {
                int yeniElemanSayisi = elemanSayisi + 8 - sonKarakterSayisi;
                byte[] yeniMesaj = new byte[yeniElemanSayisi];



                for (int i = 0; i < mesaj.Length; i++)
                {
                    yeniMesaj[i] = mesaj[i];
                }

                for (int i = 0; i < 8 - sonKarakterSayisi; i++)
                {
                    yeniMesaj[mesaj.Length + i] = 0;
                }

                mesaj = yeniMesaj;
            }

            return mesaj;



        }



        public byte[] ThreadFunction(ulong[] geciciDizi, ulong anahtar, int blokSayisi)
        {
            byte[] sonucDizisi = new Byte[blokSayisi * 8];

            for (int i = 0; i < geciciDizi.Length; i++)
            {
                ulong geciciSonuc = BlokSifreleme(geciciDizi[i], anahtar);
                byte[] gecici = baytlaraCevir(geciciSonuc);
                for (int j = 0; j < 8; j++)
                {
                    sonucDizisi[8 * i + j] = gecici[j];
                }
            }

            return sonucDizisi;
        }

        public byte[] sifreleme(string mesaj, ulong anahtar)
        {
            byte[] sifrelenecek = byteDizisineCevir(mesaj);
            byte[] sifrelenmisMesaj = new byte[sifrelenecek.Length];

            for (int i = 0; i < sifrelenecek.Length / 8; i++)
            {
                byte[] gecici = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    gecici[j] = sifrelenecek[i * 8 + j];
                }
                ulong blok = baytDizisi64BiteCevir(gecici);
                ulong geciciSonuc = BlokSifreleme(blok, anahtar);
                byte[] bitler = baytlaraCevir(geciciSonuc);
                for (int k = 0; k < 8; k++)
                {
                    sifrelenmisMesaj[i * 8 + k] = bitler[k];
                }
            }

            return sifrelenmisMesaj;
        }

        public void dosyayaYaz(string sifrelenmis)
        {
            string dosya_yolu = @"CipherText.txt";

            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sifrelenmis);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        class Program
        {


            static void Main(string[] args)
            {

                DesSifreleme des = new DesSifreleme();

                Stopwatch Watch1 = new Stopwatch();
                Stopwatch Watch2 = new Stopwatch();

                Watch1.Start();

                string text0 = "Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World!";

                string Anahtar0 = "Anahtar1";
                byte[] anahtarDizisi0 = des.byteDizisineCevir(Anahtar0);
                ulong decimalAnahtar0 = des.baytDizisi64BiteCevir(anahtarDizisi0);


                byte[] byteDizisi0 = des.byteDizisineCevir(text0);
                byte[] TümBloklar0 = des.BlokTamamlama(byteDizisi0);

                int BlokSayisi0 = TümBloklar0.Length / 8;

                ulong[] ulongDizisi0 = new ulong[BlokSayisi0];
                for (int i = 0; i < TümBloklar0.Length / 8; i++)
                {
                    byte[] gecici0 = new byte[8];
                    for (int j = 0; j < 8; j++)
                    {
                        gecici0[j] = TümBloklar0[i * 8 + j];
                    }
                    ulongDizisi0[i] = des.baytDizisi64BiteCevir(gecici0);

                }

                byte[] sonucDizisi0 = new Byte[BlokSayisi0 * 8];

                for (int i = 0; i < ulongDizisi0.Length; i++)
                {
                    ulong geciciSonuc = des.BlokSifreleme(ulongDizisi0[i], decimalAnahtar0);
                    byte[] gecici = des.baytlaraCevir(geciciSonuc);
                    for (int j = 0; j < 8; j++)
                    {
                        sonucDizisi0[8 * i + j] = gecici[j];
                    }
                }

                string cipherText0 = "";
                foreach (byte b in sonucDizisi0)
                {
                    cipherText0 += b;
                    cipherText0 += "   ";
                }
                Console.WriteLine(cipherText0);
                Console.WriteLine("*************************************************************");

                Watch1.Stop();


                Watch2.Start();
                // MULTİTHREAD PROGRAMMING

                string text = des.dosyadanOku();
                string Anahtar = "Anahtar1";
                byte[] anahtarDizisi = des.byteDizisineCevir(Anahtar);
                ulong decimalAnahtar = des.baytDizisi64BiteCevir(anahtarDizisi);


                byte[] byteDizisi = des.byteDizisineCevir(text);
                byte[] TümBloklar = des.BlokTamamlama(byteDizisi);

                int BlokSayisi = TümBloklar.Length / 8;

                ulong[] ulongDizisi = new ulong[BlokSayisi];
                for (int i = 0; i < TümBloklar.Length / 8; i++)
                {
                    byte[] gecici = new byte[8];
                    for (int j = 0; j < 8; j++)
                    {
                        gecici[j] = TümBloklar[i * 8 + j];
                    }
                    ulongDizisi[i] = des.baytDizisi64BiteCevir(gecici);

                }

                int birinci = 0;
                int ikinci = 0;
                int ucuncu = 0;
                int dorduncu = 0;


                int threatSayisi = 4;
                if (ulongDizisi.Length < 4)
                {
                    threatSayisi = ulongDizisi.Length;
                    if (threatSayisi == 1)
                        birinci = 1;
                    if (threatSayisi == 2)
                    {
                        birinci = 1;
                        ikinci = 1;
                    }
                    if (threatSayisi == 3)
                    {
                        birinci = 1;
                        ikinci = 1;
                        ucuncu = 1;
                    }

                }
                else
                {
                    birinci = ulongDizisi.Length / 4;
                    ikinci = ulongDizisi.Length / 4;
                    ucuncu = ulongDizisi.Length / 4;
                    dorduncu = ulongDizisi.Length / 4;

                    if (threatSayisi == 4 && ulongDizisi.Length % 4 != 0)
                    {
                        if (ulongDizisi.Length % 4 == 1)
                        {
                            birinci += 1;
                        }
                        else if (ulongDizisi.Length % 4 == 2)
                        {
                            birinci += 1;
                            ikinci += 1;
                        }
                        else if (ulongDizisi.Length % 4 == 3)
                        {
                            birinci += 1;
                            ikinci += 1;
                            ucuncu += 1;

                        }

                    }
                }

                ulong[] birinciParca = new ulong[birinci];
                ulong[] ikinciParca = new ulong[ikinci];
                ulong[] ucuncuParca = new ulong[ucuncu];
                ulong[] dorduncuParca = new ulong[dorduncu];

                for (int i = 0; i < ulongDizisi.Length; i++)
                {
                    if (i < birinci)
                        birinciParca[i] = ulongDizisi[i];
                    else if (i >= birinci && i < birinci + ikinci)
                        ikinciParca[i - birinci] = ulongDizisi[i];
                    else if (i >= birinci + ikinci && i < birinci + ikinci + ucuncu)
                        ucuncuParca[i - (birinci + ikinci)] = ulongDizisi[i];
                    else if (i >= birinci + ikinci + ucuncu && i < birinci + ikinci + ucuncu + dorduncu)
                        dorduncuParca[i - (birinci + ikinci + ucuncu)] = ulongDizisi[i];
                }



                byte[] value1 = null;
                Thread thread1 = new Thread(() => { value1 = des.ThreadFunction(birinciParca, decimalAnahtar, birinciParca.Length); });
                thread1.Start();

                byte[] value2 = null;
                Thread thread2 = new Thread(() => { value2 = des.ThreadFunction(ikinciParca, decimalAnahtar, ikinciParca.Length); });
                thread2.Start();

                byte[] value3 = null;
                Thread thread3 = new Thread(() => { value3 = des.ThreadFunction(ucuncuParca, decimalAnahtar, ucuncuParca.Length); });
                thread3.Start();

                byte[] value4 = null;
                Thread thread4 = new Thread(() => { value4 = des.ThreadFunction(dorduncuParca, decimalAnahtar, dorduncuParca.Length); });
                thread4.Start();

                thread1.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();


                byte[] sonucDizisi = new byte[TümBloklar.Length];

                for (int i = 0; i < sonucDizisi.Length; i++)
                {
                    if (i < value1.Length)
                        sonucDizisi[i] = value1[i];
                    else if (i >= value1.Length && i < (value1.Length + value2.Length))
                        sonucDizisi[i] = value2[i - value1.Length];
                    else if (i >= (value1.Length + value2.Length) && i < (value1.Length + value2.Length + value3.Length))
                        sonucDizisi[i] = value3[i - (value1.Length + value2.Length)];
                    else if (i >= (value1.Length + value2.Length + value3.Length) && i < (value1.Length + value2.Length + value3.Length + value4.Length))
                        sonucDizisi[i] = value4[i - (value1.Length + value2.Length + value3.Length)];

                }
                
                string cipherText = "";
                foreach (byte b in sonucDizisi)
                {
                    cipherText +=  b;
                    cipherText +="   ";
                }
                  
                des.dosyayaYaz(cipherText);

                Watch2.Stop();

                string abc = Convert.ToBase64String(sonucDizisi);
                //Console.WriteLine(abc);
                var dizi = Convert.FromBase64String(abc);
                

                Console.WriteLine("Watch1 : {0}", Watch1.ElapsedTicks);
                Console.WriteLine("Watch2 : {0}", Watch2.ElapsedTicks);
                


















            }


        }
    }
}