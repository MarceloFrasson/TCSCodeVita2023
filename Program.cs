using System;

namespace ordenaCaixas
{
    class Program
    {
        public static int totalCaixas = 0;
        public static int caixaMaior = 0;
        public static int[] listaCaixas;
        public static int[] gabarito;
        public static int gabaritoEsforco = 0;
        public static int[] troca01;
        public static int[] erradas;
        public static int troca01Esforco = 0;

        public static int posicaoCaixaMaior = 0;
        public static int valCaixaMaior = 0;
        public static int posicaoCaixaMenor = 0;
        public static int valCaixaMenor = 0;

        public static Boolean showPrint = true;

        enum Tipo
        {
            Maior,
            Menor
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] != "")
                    showPrint = (args[0].ToLower() == "true");
            }
            if (!entradaDados()) return;
            if (!validaEntrada()) return;
            DateTime inicio = DateTime.Now;
            processaCaixas();
            Console.WriteLine(string.Format("\tTempo utilizado: {0}", (DateTime.Now - inicio).ToString("c")));
            fimProcesso();
        }

        private static int esforco(int caixa1, int caixa2, int caixaM)
        {
            var inverte = 1;
            var troca1 = caixa1 * caixa2;

            if (caixa1 > caixa2)
            {
                var aux = caixa1;
                caixa1 = caixa2;
                caixa2 = aux;
                inverte = -1;
                if (showPrint)
                    Console.WriteLine("inverteu....");
            }

            var troca2 = caixa1 * caixaM;
            troca2 += caixa2 * caixaM; // caixa2 sendo maior, troca só 1 vez
            troca2 += caixa1 * caixaM;

            /*** 
             * Retorno da funcao
             * TRUE: troca1 é maior, ou seja, fazer a troca usando a caixa menor, começar sempre pela menor
             * FALSE:troca1 é menor ou igual a troca com a caixa menor, ou seja, compensa fazer a troca simples
             * ****/
            if (showPrint) Console.WriteLine(string.Format("\tTroca 1: {0}\t\t\tTroca 2: {1}", troca1, troca2));

            int resultado = (troca1 > troca2) ? (1 * inverte) : 0;

            return resultado;
        }

        private static Boolean validaEntrada()
        {
            if (totalCaixas > 50)
            {
                Console.WriteLine(string.Format("\tMáximo de caixas ultrapassado. Máximo: 50 Informado: {0}", totalCaixas));
                return false;
            }
            for (int i = 0; i < totalCaixas; i++)
            {
                if (listaCaixas[i] > 1000)
                {
                    Console.WriteLine(string.Format("\tPeso máximo da caixa ultrapassado. Máximo: 1000 Informado: {0}", listaCaixas[i]));
                    return false;
                }
            }
            return true;
        }

        private static void fimProcesso()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Pressione uma tecla para Sair....");
            Console.ReadKey();
        }

        private static void processaCaixas()
        {
            posicaoCaixaMaior = 0;
            valCaixaMaior = listaCaixas[0];

            posicaoCaixaMenor = 0;
            valCaixaMenor = listaCaixas[0];


            for (int i = 0; i < totalCaixas; i++)
            {
                var caixa_i = listaCaixas[i];
                posicaoCaixaMaior = (caixa_i > valCaixaMaior) ? i : posicaoCaixaMaior;
                valCaixaMaior = (caixa_i > valCaixaMaior) ? caixa_i : valCaixaMaior;

                posicaoCaixaMenor = (caixa_i < valCaixaMenor) ? i : posicaoCaixaMenor;
                valCaixaMenor = (caixa_i < valCaixaMenor) ? caixa_i : valCaixaMenor;

            }
            posicaoCaixaMaior++;

            if (showPrint)
            {
                Console.WriteLine(string.Format("totalCaixas: {0}", totalCaixas));
                Console.WriteLine(string.Format("caixaMaior(Onde Quer): {0}", caixaMaior));
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(string.Format("posicaoCaixaMaior: {0}", posicaoCaixaMaior));
                Console.WriteLine(string.Format("valCaixaProcessada: {0}", valCaixaMaior));
                Console.WriteLine("");
                Console.WriteLine(string.Format("posicaoCaixaMenor: {0}", posicaoCaixaMenor));
                Console.WriteLine(string.Format("valCaixaMenor: {0}", valCaixaMenor));
                Console.WriteLine("");
            }
            geraGabarito();
            //geraTrocas();
            geraTrocas_02();
            if (showPrint)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(string.Format("gabaritoEsforco: {0}", gabaritoEsforco));
                for (int i = 0; i < totalCaixas; i++)
                {
                    Console.WriteLine(string.Format("Caixa n.{0}  :{1}", i + 1, gabarito[i]));
                }
                Console.WriteLine("...................");
                Console.WriteLine("");
                Console.WriteLine(string.Format("troca01Esforco: {0}", troca01Esforco));
                for (int i = 0; i < totalCaixas; i++)
                {
                    Console.WriteLine(string.Format("Caixa n.{0}  :{1}", i + 1, troca01[i]));
                }
                Console.WriteLine("...................");
            }
            var esforco = gabaritoEsforco > troca01Esforco ? troca01Esforco : gabaritoEsforco;
            Console.WriteLine(string.Format("Esforco: {0}", esforco));
        }

        private static void geraTrocas()
        {
            Console.WriteLine("............................geraTrocas........");


            troca01 = new int[totalCaixas];
            listaCaixas.CopyTo(troca01, 0);
            erradas = new int[totalCaixas];
            int posCmenor = 0;
            troca01Esforco = 0;

            int ierrada = 1;
            int ierrada_anterior = 1;
            while (ierrada > 0)
            {
                ierrada_anterior = ierrada;
                ierrada = buscaErradas();
                if (ierrada_anterior == ierrada)
                {
                    Console.WriteLine("........CAIU NO LOOP INFINITO...........");
                    ierrada = 0;
                    troca01Esforco = gabaritoEsforco;
                }
                if (ierrada > 1)
                {
                    posCmenor = buscaPosCaixa(troca01, Tipo.Menor);
                    var posCmenorOriginal = posCmenor;
                    var caixa1 = troca01[erradas[0]];
                    var caixa2 = troca01[erradas[1]];
                    var caixaM = troca01[posCmenor];
                    var esforcotroca = esforco(caixa1, caixa2, caixaM);

                    if (erradas[0] == posCmenor || erradas[1] == posCmenor)
                    {
                        esforcotroca = 0;
                    }

                    if (esforcotroca == 0)
                    {
                        //Console.WriteLine(" compensa fazer a troca simples: caixa1 x caixa2 ");
                        troca01Esforco += troca(troca01, erradas[0], erradas[1]);
                    }
                    else
                    {
                        if (esforcotroca > 0)
                        {
                            //faz a troca usando a caixa menor, NAO PRECISA INVERTER
                            troca01Esforco += troca(troca01, erradas[0], posCmenor);
                            posCmenor = erradas[0];
                            troca01Esforco += troca(troca01, erradas[1], posCmenor);
                            posCmenor = erradas[1];
                            if (troca01[posCmenorOriginal] != gabarito[posCmenorOriginal])
                                troca01Esforco += troca(troca01, posCmenorOriginal, posCmenor);
                        }
                        else
                        {
                            //faz a troca usando a caixa menor, TEM QUE INVERTER A ORDEM
                            troca01Esforco += troca(troca01, erradas[1], posCmenor);
                            posCmenor = erradas[1];
                            troca01Esforco += troca(troca01, erradas[0], posCmenor);
                            posCmenor = erradas[0];
                            if (troca01[posCmenorOriginal] != gabarito[posCmenorOriginal])
                                troca01Esforco += troca(troca01, posCmenorOriginal, posCmenor);
                        }
                    }
                }
            }
        }

        private static void geraTrocas_02()
        {
            Console.WriteLine("............................geraTrocas_02.....");

            troca01 = new int[totalCaixas];
            listaCaixas.CopyTo(troca01, 0);
            erradas = new int[totalCaixas];
            int posCmenor = 0;
            troca01Esforco = 0;

            int ierrada = 1;
            int ierrada_anterior = 1;
            while (ierrada > 0)
            {
                ierrada_anterior = ierrada;
                ierrada = buscaErradas();
                if (ierrada_anterior == ierrada)
                {
                    Console.WriteLine("........CAIU NO LOOP INFINITO...........");
                    ierrada = 0;
                    troca01Esforco = gabaritoEsforco;
                }
                if (ierrada > 1)
                {
                    posCmenor = buscaPosCaixa(troca01, Tipo.Menor);
                    var posCmenorOriginal = posCmenor;
                    var caixa1 = troca01[erradas[0]];
                    erradas[1] = buscaPosCaixaValor(gabarito, caixa1);
                    var caixa2 = troca01[erradas[1]];
                    var caixaM = troca01[posCmenor];
                    var esforcotroca = esforco(caixa1, caixa2, caixaM);

                    if (erradas[0] == posCmenor || erradas[1] == posCmenor)
                    {
                        esforcotroca = 0;
                    }

                    if (esforcotroca == 0)
                    {
                        //Console.WriteLine(" compensa fazer a troca simples: caixa1 x caixa2 ");
                        troca01Esforco += troca(troca01, erradas[0], erradas[1]);
                    }
                    else
                    {
                        if (esforcotroca > 0)
                        {
                            //faz a troca usando a caixa menor, NAO PRECISA INVERTER
                            troca01Esforco += troca(troca01, erradas[0], posCmenor);
                            posCmenor = erradas[0];
                            troca01Esforco += troca(troca01, erradas[1], posCmenor);
                            posCmenor = erradas[1];
                            if (troca01[posCmenorOriginal] != gabarito[posCmenorOriginal])
                                troca01Esforco += troca(troca01, posCmenorOriginal, posCmenor);
                        }
                        else
                        {
                            //faz a troca usando a caixa menor, TEM QUE INVERTER A ORDEM
                            troca01Esforco += troca(troca01, erradas[1], posCmenor);
                            posCmenor = erradas[1];
                            troca01Esforco += troca(troca01, erradas[0], posCmenor);
                            posCmenor = erradas[0];
                            if (troca01[posCmenorOriginal] != gabarito[posCmenorOriginal])
                                troca01Esforco += troca(troca01, posCmenorOriginal, posCmenor);
                        }
                    }
                }
            }
        }

        private static int buscaErradas()
        {
            int ierrada = 0;
            Array.Fill(erradas, -1);

            for (int i = 0; i < totalCaixas; i++)
            {
                if (troca01[i] != gabarito[i])
                {
                    erradas[ierrada] = i;
                    ierrada++;
                }
            }
            return ierrada;
        }

        private static int buscaPosCaixa(int[] arrayBusca, Tipo tipoCaixa)
        {
            int maior = 0;
            int menor = 0;

            for (int i = 0; i < arrayBusca.Length; i++)
            {
                maior = arrayBusca[maior] < arrayBusca[i] ? i : maior;
                menor = arrayBusca[menor] > arrayBusca[i] ? i : menor;
            }

            if (tipoCaixa == Tipo.Menor) { return menor; }
            return maior;
        }

        private static int buscaPosCaixaValor(int[] arrayBusca, int valorCaixa)
        {
            for (int i = 0; i < arrayBusca.Length; i++)
            {
                if (arrayBusca[i] == valorCaixa)
                {
                    return i;
                }                
            }
            return 0;
        }

        

        private static void geraGabarito()
        {
            Console.WriteLine("............................geraGabarito......");

            int esforco = 0;
            int esforcoTotal = 0;
            gabarito = new int[totalCaixas];

            listaCaixas.CopyTo(gabarito, 0);

            if (posicaoCaixaMaior != caixaMaior)
            {
                esforcoTotal += troca(gabarito, posicaoCaixaMaior, caixaMaior, true);
            }
            for (int j = 0; j < totalCaixas; j++)
            {
                esforco = 0;
                for (int i = 0; i < totalCaixas; i++)
                {
                    if (compara(gabarito, i, i + 1))
                    {
                        esforco = troca(gabarito, i, i + 1);
                        esforcoTotal += esforco;
                    }
                    else if (compara(gabarito, i, i + 2))
                    {
                        esforco = troca(gabarito, i, i + 2);
                        esforcoTotal += esforco;
                    }
                }
                if (esforco == 0) { j = totalCaixas; }
            }
            gabaritoEsforco = esforcoTotal;
        }

        private static bool compara(int[] arraycompara, int i, int v, Boolean desprezaMaior = false)
        {
            if (desprezaMaior)
            {
                if (v < totalCaixas)
                    if (arraycompara[i] > arraycompara[v])
                        return true;
            }
            else
            {
                if (v < totalCaixas)
                    if (i != caixaMaior - 1)
                        if (v != caixaMaior - 1)
                            if (arraycompara[i] > arraycompara[v])
                                return true;
            }
            return false;
        }

        private static int troca(int[] lista, int p1, int p2, Boolean posicao = false)
        {
            p1 = posicao ? p1 - 1 : p1;
            p2 = posicao ? p2 - 1 : p2;
            int aux = 0;
            int esforco = 0;
            esforco = lista[p1] * lista[p2];
            if (showPrint)
                Console.WriteLine(string.Format("Troca Caixas => {0} x {1} = {2}", lista[p1], lista[p2], esforco));

            aux = lista[p1];
            lista[p1] = lista[p2];
            lista[p2] = aux;
            return esforco;
        }

        private static Boolean entradaDados()
        {
            try
            {
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------ORDENACAO DE CAIXAS---------------------------------");
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("--------------------------------------------------------------------------------------");

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("\tInforme a quantidade de caixas e onde deseja a caixa de maior peso");
                Console.WriteLine("\t(Separe os valores por um espaço)");
                var linha1 = Console.ReadLine();
                var campos1 = linha1.Trim().Split(" ");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(string.Format("\tInforme o peso das {0} caixas", campos1[0]));
                Console.WriteLine("\t(Separe os valores por um espaço)");
                var linha2 = Console.ReadLine();
                var campos2 = linha2.Trim().Split(" ");
                Console.WriteLine("");
                Console.WriteLine("");
                totalCaixas = int.Parse(campos1[0]);
                caixaMaior = int.Parse(campos1[1]);
                listaCaixas = new int[totalCaixas];
                int length = int.Parse(campos1[0]);
                if (showPrint)
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Os valores são: ");
                    Console.WriteLine(string.Format("quantidade de caixas: {0}", campos1[0]));
                    Console.WriteLine(string.Format("onde deseja a caixa de maior peso: {0}", campos1[1]));
                    Console.WriteLine("");
                    for (int i = 0; i < length; i++)
                    {
                        Console.WriteLine(string.Format("Caixa n.{0}  :{1}", i + 1, campos2[i]));
                        listaCaixas[i] = int.Parse(campos2[i]);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        listaCaixas[i] = int.Parse(campos2[i]);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Os valores informados estão fora do padrão necessário...");
                Console.WriteLine("");
                Console.WriteLine("");
                return false;
            }
        }
    }
}
