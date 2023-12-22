namespace WebApplication1.Services;

public class BCGameService
{
    private byte bull;
    private byte cow;

    private readonly RedisService redis;

    public BCGameService(RedisService redis)
    {
        this.redis = redis;
    }
    /// <summary>
    /// Сreate num sequence for player
    /// </summary>
    public string GenerateNum()
    {
        int count = 0;
        var quiz = "";
        while (count != 4)
        {
            int num = Random.Shared.Next(0, 10);

            if (!quiz.Contains(num.ToString()))
            {
                quiz += num.ToString();
                count++;
            }
        }
        return quiz;
    }

    private bool CheckCorrect(string input)
    {
        if (input.Length != 4) return false;
        foreach (var item in input)
        {
            if (!int.TryParse(item.ToString(), out int val))
            {
                return false;
            }
        }
        return true;
    }

    private bool CompareStrings(string input, string quiz)
    {        
        if (input.Equals(quiz))
            return true;

        bull = 0;
        cow = 0;

        for (int i = 0; i < 4; i++)
        {
            if (quiz.Contains(input[i]))
            {
                var indexOfResult = quiz.IndexOf(input[i]);

                if (indexOfResult == i)
                    bull++;
                else
                    cow++;
            }
        }

        return bull == 4;
    }

    public AnswerGame CheckAnswer(string input, string id)
    {
        var quiz = redis.GetUserQuiz(id).Result;

        if (quiz is null)
            return new AnswerGame(false, 0, 0);

        var isCorrect = CheckCorrect(input);
        if (isCorrect)
        {
            var isWin = CompareStrings(input, quiz);
            if (!isWin)
            {
                return new AnswerGame(false, bull, cow);
            }
            return new AnswerGame(true);
        }
        throw new ArgumentException();

    }
}


public struct AnswerGame
{
    public bool IsWin { get; init; }
    public int[]? Nums { get; init; }
    public AnswerGame(bool r, int b = -1, int c = -1)
    {
        IsWin = r;

        if (!r)
        {
            Nums = new int[2];
            Nums[0] = b;
            Nums[1] = c;
        }
    }
}