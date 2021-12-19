using System;

namespace remainder_of_the_didvision
{
    class Program
    {
		public static int e = 0b0, ost = 0b0;
		static void Main()
        {

			int Ne = 0,g = 0;
			double Pi = 0, pe = 0.1; // Pe - вероятность на бит, Pi - вероятность ошибки
			double E = 2; // точность
			double N = 1000; // число опытов
			string l = "       ";
			int count = 0;

            while (E < 0 || E > 1)
            {
				Console.WriteLine("Введите точность Е. Пример: (0,01) : \r\n");
				l = Console.ReadLine();
				E = Convert.ToDouble(l);
				if (E < 0 || E > 1) {
					Console.WriteLine("Не верный формат");
				}
			}
			while (count < l.Length) {
				Console.WriteLine("Введите g(x). Пример: (1011) : \r\n");
				l = Console.ReadLine();
				for (int i = 0; i < l.Length; i++)
				{
					if (l[i] == 49 || l[i] == 48 )
					{
						count++;
					}
				}
                if (count == l.Length)
                {
					break;
                }
                else
                {
					Console.WriteLine("Не верный формат");
				}
				count = 0;
			}
			for (int i = 0; i < l.Length; i++)
			{
				g = g | (Convert.ToInt32(l[i]) - 48);
				g = g << 1;
			}
			g = g >> 1;

			for (int j = 0; j < 10; j++, pe = pe + 0.09)
			{
				N = pe / ((1 - 0.9) * E * E);

				for (double i = N; i > 0; i--)
				{
					Probability_of_error(pe, g); //  вызов функции кодера, декодера
					if (Verdict_dec(ost, e) == 0)
					{ // вызов вердикта декодера
						Ne++;
					}
					ost = 0;
					e = 0;
				}
				Pi = ((float)Ne / (float)N) * 100; // вычисление вероятности
				Console.WriteLine("Вероятность ошибки = {0:F9}% ", Pi);
				Console.WriteLine("При Pe = {0:F2}", pe);
				
			}
			pe = 0.99;
			N = pe / ((1 - 0.9) * E * E);

			for (double i = N; i > 0; i--)
			{
				Probability_of_error(pe, g); //  вызов функции кодера, декодера
				if (Verdict_dec(ost, e) == 0)
				{ // вызов вердикта декодера
					Ne++;
				}
				ost = 0;
				e = 0;
			}
			Pi = ((float)Ne / (float)N) * 100; // вычисление вероятности
			Console.WriteLine("Вероятность ошибки = {0:F9}% ", Pi);
			Console.WriteLine("При Pe = {0:F2}", pe);
		}

		// функция кодера и декодера
		static void Probability_of_error(double pe, int g)
		{
			int m = 0b1011, c = 0;
			//int g = 0b1011, c = 0;
			int n = 7, r = 3;
			int a = 0, b = 0;

			m = Gen_v_err(4, 0.5);

			c = (m << r); // Сдвиг С на R
			c = Rotd(c, g, n); // Нахождение остатка
			a = m << r | c; // Нахождение А

			e = Gen_v_err(n, pe); // Генерация ошибок

			b = a ^ e;

			ost = Rotd(b, g, n); // Нахождение остатка

		}

		// функция нахождения остатка
		static int Rotd(int c, int g, int n)
		{
			int difference = 0; ;
			int count_b = 0, count_a = 0, count = 0, cop_g = 0, cop_c = 0;

			while (c > 0)
			{

				cop_g = g;
				cop_c = c;
				for (int i = 0; i != n; i++)
				{ // цикл нахождения размера С и G
					if ((cop_c & 1) == 1)
					{
						count_a = i;
					}
					if ((cop_g & 1) == 1)
					{
						count_b = i;
					}
					cop_c = cop_c >> 1;
					cop_g = cop_g >> 1;
				}
				count_a++;
				count_b++;
				count = count_a;
				difference = count_a - count_b; // Разница С и G

				cop_g = g;
				if (difference < 0)
				{ // Если разница меньше 0, то остаток = С
					return c;
				}
				else
				{
					cop_g = g << difference; // Сдвиг делителя на разницу С и G
					c = c ^ cop_g; // Сложение XOR
				}
			}
			return c;
		}

		// функция вынесения вердикта
		static int Verdict_dec(int ost, int e)
		{
			if (ost == 0 && e != 0)
			{
				//printf("Декодер принял не верное решение\n\r");
				return 0;
			}
			return 1;
		}

		// функция генерации вектора ошибок
		static int Gen_v_err(int n, double pe)
		{
			Random rnd = new Random();
			int v_error = 0;
			double P = 0;

			for (int i = n; i > 0; i--)
			{
				//p = (rand() % 100) / (100 * 1.0);
				//P = (double)rand() / (double)RAND_MAX;
				P = rnd.NextDouble();
				if (P > pe)
				{
					v_error = v_error | 0;
				}
				else
				{
					v_error = v_error | 1;
				}
				v_error = v_error << 1;
			}
			return v_error;
		}

	}
}
