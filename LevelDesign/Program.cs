using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;


/*
 * ========= Entendendo o JSON levelSetup =========
 * 
 * 
 * playerNickName: apelido do jogador
 * 
 * totalTimeSetup: (SEGUNDOS) tempo total de jogo selecionado, porém o tempo real de jogo é determinado pelo número de bolas. Esse tempo será utilizado para calcular o número total de bolas baseado na média entre o minBallInterval e o maxBallInterval. Regras sobre erros sucessivos podem ser aplicadas para terminar o jogo antes, assim como a manutenção de um nível alto de acertos podem fazer o jogo se prolongar.
 * 
 * minBallInterval: (SEGUNDOS) Menor intervalo possível entre uma caída de bola e outra.
 * 
 * maxBallInterval: (SEGUNDOS) Maior intervalo possível entre uma caída de bola e outra.
 *  * 
 * minSpeed: (NÚMERO INTEIRO - UNIDADE ALEATÓRIA) Menor velocidade possível de uma bola.
 * 
 * maxSpeed: (NÚMERO INTEIRO - UNIDADE ALEATÓRIA) Maior velocidade possível de uma bola.
 * 
 * weights: (ARRAY COM NÚMEROS INTEIROS QUE REPRESENTAM %) É um array com 4 itens representando as posições que as bolas são lançadas. Cada valor é a % de bolas que serão escolhidas para aquele alvo, portanto a soma dos 4 valores deve ser igual a 100. Isso permite que o jogo escolha mais bolas de um lado que do outro, mais no centro que nos cantos ou vice-versa.
 * 
 * allowGravity: (BOLEANO) Padrão False. Quando ativado adiciona o valor da aceleração da gravidade na velocidade das bolas.
 * 
 * allowRepeated: (BOLEANO) Padrão True. Quando ativado, permite que uma bola seja lançada após a outra na mesma posição.
 * 
 * delayRepeated: (DECIMAL QUE REPRESENTA %) Acontece quando o allowRepeated está habilitado. Quando uma bola é lançada na mesma posição que sua anterior, esse valor será multiplicado ao seu intervalo. Exemplo: Se uma bola for lançada na posição 3 com intervalo de 1.00 e a anterior também estiver sido lançada na posição 3, esse valor será multiplicado pelo delayRepeater. Se o valor for 1, o intervalo será multiplicado por 1 e nada acontece. Se o valor for maior que 1, 1.2 por exemplo, significa que o intervalo dessa bola aumentará 20%. Se o valor for menor que 1, 0.80 por exemplo, significa que o intervalo dessa bola diminuirá 20% do valor que foi atribuido.
 * 
 * allowOpositeCorner: (BOLEANO) Padrão True. Quando ativado permite que uma bola seja lançada no canto oposto de sua anterior. Exemplo, se uma bola for lançada na posição 1 e o allowOpositeCorner for False, a próxima bola não será lançada na posição 4. O mesmo acontece se a bola for lançada na posição 4, a próxima não poderá ser lançada na posição 1.
 * 
 * delayOpositeCorner: (DECIMAL QUE REPRESENTA %) Acontece quando o allowOpositeCorner está habilitado. Quando uma bola é lançada em um canto (posição 1 ou 4) e a bola anterior foi lançada no outro canto (posição 4 ou 1), esse valor será multiplicado ao intervalo dessa bola, exemplo: se o delayRepeated estiver em 1.2 e se a bola lançada for no canto oposoto de sua anterior e o intervalo escolhido for de 1.00 segundo. Esse 1.00 segundo será multiplicado por 1.2, acrescentando 20% no seu intervalo e essa bola vai demorar mais para aparecer.
 * 
 * allowDifferentSize: (BOLEANO) Padrão False. Se estiver habilitado, as bolas poderão ter tamanhos diferentes.
 * 
 * minBallSize: (INTEIRO, UNIDADE ALEATÓRIA) Menor tamanho que a bola pode ter. Se o allowDifferentSize estiver False, o valor dessa variável será utilizada para padronizar o tamanho de todas as bolas.
 * 
 * maxBallSite: (INTEIRO, UNIDADE ALEATÓRIA) Maior tamanho que a bola pode ter.
 * 
 * ballTransparency: (DECIMAL QUE REPRESENTA %) % de transparência das bolas, sendo 0.00 sólidas e 0.99 quase transparente.
 * 
 * colorMode: (INTEIRO) São padrões de cores estabelecidas. Por exemplo: alto contraste, baixo contraste, grayscale, claro, escuro, etc.
 *  
 */


public class LevelSetup
{
    [JsonPropertyName("player")]
    public string playerNickName {  get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int totalTimeSetup { get; set; }

    [JsonConverter(typeof(StringToFloatConverter))]
    public float minBallInterval { get; set; }

    [JsonConverter(typeof(StringToFloatConverter))]
    public float maxBallInterval { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int minSpeed { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int maxSpeed { get; set; }

    [JsonConverter(typeof(StringToArrayConverter))]
    public string[] weights {  get; set; }

    [JsonConverter(typeof(StringToBoolConverter))]
    public bool allowGravity { get; set; }

    [JsonConverter(typeof(StringToBoolConverter))]
    public bool allowRepeated { get; set; }

    [JsonConverter(typeof(StringToFloatConverter))]
    public float? delayRepeated { get; set; }

    [JsonConverter(typeof(StringToBoolConverter))]
    public bool allowOpositeCorner { get; set; }

    [JsonConverter(typeof(StringToFloatConverter))]
    public float? delayOpositeCorner { get; set; }

    [JsonConverter(typeof(StringToBoolConverter))]
    public bool allowDifferentSize { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int? minBallSize { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int? maxballSize { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int ballTransparency { get; set; }

    [JsonConverter(typeof(StringToIntConverter))]
    public int colorMode { get; set; }
}




/* ========== Ball ==========
 * Cada bola tem sua posição e tempo.
 * position: é a coluna que será lançada (1, 2, 3, 4)
 * time: (MILISEGUNDOS) é o tempo em milisegundos, a partir da contagem do início do jogo, que ela será lançada.
 * 
 * Exemplo: [{"position": 1, "time": 520}, {"position": 4, "time": 1200}]
 * A bola 1 será lançada na posição 1, 520 milissegundos após o início do jogo.
 * A bola 2 será lançada na posição 4, 1200 milissegundos após o início do jogo.
 * ... e assim por diante...
 */

public class Ball
{
    public int position { get; set; }
    public float time { get; set; }
}


class Program
{
    static void Main(string[] args)
    {
        string jsonPath = "D:\\Projects\\OPENHEAL\\test\\LevelDesign\\LevelDesign\\levelSetup.json"; //o caminho do arquivo será substituido pela chamada da API.
        List<Ball> balls = new List<Ball>(); // essá será a lista de bolas lançadas na partida, conforme descrito no objeto "Ball".


        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"O arquivo não foi encontrado: {jsonPath}");
        }


        string jsonString = File.ReadAllText(jsonPath);
        
        LevelSetup levelSetup = JsonSerializer.Deserialize<LevelSetup>(jsonString); // Carrega o JSON no objeto LevelSetup


        balls = GenerateBallsSequence(levelSetup); // gera a sequencia de bolas lançadas

        int outputTotalBalls = balls.Count; // número real de bolas da partida, esse valor volta para o adm
        Ball lastBall = balls.Last();
        float outputTotalTime = (int)(lastBall.time / 1000); // tempo real do jogo em segundos, esse valor volta para o adm


        // Só para ver como fica
        Console.WriteLine($"Tempo do jogo: {outputTotalTime}seg");
        Console.WriteLine($"Total de bolas: {outputTotalBalls}");
        Console.WriteLine();
        Console.WriteLine("============================");
        Console.WriteLine();
        Console.WriteLine("Lista de bolas");
        foreach (Ball ball in balls)
        {
            Console.WriteLine($"Posição: {ball.position} - tempo: {ball.time}");
        }

    }


    // GERAR A SEQUENCIA DE BOLAS COM POSIÇÃO E TEMPO
    static List<Ball> GenerateBallsSequence(LevelSetup levelSetup)
    {
        int totalBalls = CalculateTotalBalls(levelSetup.totalTimeSetup, levelSetup.minBallInterval, levelSetup.maxBallInterval);
        List<int> weightedChoices = GenerateWeightedChoices(totalBalls, levelSetup.weights);
        int totalGameTime = 0;
        List<Ball> balls = new List<Ball>();


        foreach (int choice in weightedChoices)
        {
            int randonInterval = GenerateRandomInterval(levelSetup.minBallInterval, levelSetup.maxBallInterval); //Value in Miliseconds
            totalGameTime = totalGameTime + randonInterval;
            Ball ball = new Ball
            {
                position = choice,
                time = totalGameTime,
            };
            balls.Add(ball);
        }
        return balls;
    }


    // CALCULAR O TOTAL DE BOLAS DA PARTIDA
    static int CalculateTotalBalls(int totalTime, float minBallInterval, float maxBallInterval)
    {
        float averageFallingTime = (minBallInterval + maxBallInterval) / 2; // Calculamos a média entre os valores mínimo e máximo permitidos
        return (int)Math.Ceiling(totalTime / averageFallingTime); // Dividimos o tempo total escolhido pela média e arredondamos para o próximo inteiro para expressar o valor em segundos.
    }


    // DISTRIBUIR AS BOLAS NAS POSIÇÕES, RESPEITANDO AS % ESTABELECIDAS.
    static List<int> GenerateWeightedChoices(int totalBalls, string[] weights)
    {
        List<int> weightedChoices = new List<int>();

        
        int count1 = (int)(totalBalls * float.Parse(weights[0]) / 100); // Estabelecemos a quantidade de bolas para cada posição.
        int count2 = (int)(totalBalls * float.Parse(weights[1]) / 100);
        int count3 = (int)(totalBalls * float.Parse(weights[2]) / 100);
        int count4 = (int)(totalBalls * float.Parse(weights[3]) / 100);
        
        for (int i = 0; i < count1; i++) //Inserimos todas as posições em uma única lista
        {
            weightedChoices.Add(1);
        }

        for (int i = 0; i < count2; i++)
        {
            weightedChoices.Add(2);
        }

        for (int i = 0; i < count3; i++)
        {
            weightedChoices.Add(3);
        }

        for (int i = 0; i < count4; i++)
        {
            weightedChoices.Add(4);
        }

        ShuffleList(weightedChoices); // Randomizamos a lista

        return weightedChoices;
    }


    // GERAR UM INTERVALO ALEATÓRIO ENTRE O MÍNIMO E MÁXIMO ESTABELECIDO
    static int GenerateRandomInterval( float minInterval, float maxInverval)
    {
        Random random = new Random();
        return random.Next((int)(minInterval*1000), (int)(maxInverval*1000) + 1);
    }

    // RANDOMIZAR UMA LISTA
    static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        Random random = new Random();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }


        /*
        static int RandomWeightedChoice(List<int> choices, Random random)
        {
            int totalWeight = choices.Count;
            int selectedIndex = random.Next(totalWeight);
            return choices[selectedIndex];
        }
        */
    }
}



// BAIXO ESTÃO FUNÇÕES PARA ADEQUAR AS STRINGS DO JSON RECEBIDO PARA A PROPRIEDADE CERTA DE CADA VARIÁVEL (FLOAT, INT, BOOL, ARRAY)
public class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (int.TryParse(reader.GetString(), out int result))
            {
                return result;
            }
        }
        return 0; // Valor padrão se a conversão falhar
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class StringToFloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (float.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
        }
        return 0.0f; // Valor padrão se a conversão falhar
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class StringToArrayConverter : JsonConverter<string[]>
{
    public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var list = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    list.Add(reader.GetString());
                }
            }
            return list.ToArray();
        }
        return Array.Empty<string>(); // Valor padrão se a conversão falhar
    }

    public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStringValue(item);
        }
        writer.WriteEndArray();
    }
}

public class StringToBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string value = reader.GetString();
            if (string.Equals(value, "True", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (string.Equals(value, "False", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return false; // Default value if conversion fails
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "True" : "False");
    }
}

