do $$
    int c = 10 + 25;
    Elog.Info($"c = {c}");
$$ language plcsharp;

do $$
    int c = 1450 + 275;
    Elog.Info($"c = {c}");
$$ language plcsharp;

do $$
    double[] doublevalues = {2.25698, 2.85956, 2.85456, 0.00128, 0.00127, 2.36875};
    double min = double.MaxValue;
    for(int i = 0; i < doublevalues.Length; i++)
    {
        double value = (double)doublevalues.GetValue(i);
        min = min < value ? min : value;
    }
    Elog.Info($"Minimum value = {min}");
$$ language plcsharp;

do $$
    double[] doublevalues = {2.25698, -2.85956, 2.85456, -0.00128, 0.00127, 12.36875, -23.2354};
    double min = double.MaxValue;
    for(int i = 0; i < doublevalues.Length; i++)
    {
        double value = (double)doublevalues.GetValue(i);
        min = min < value ? min : value;
    }
    Elog.Info($"Minimum value = {min}");
$$ language plcsharp;

do $$
    string arabic = "هل تتكلم العربية؟";
    Elog.Info($"Do you speak Arabic? => {arabic}");

    string chinese = "你会说中文吗？";
    Elog.Info($"Do you speak Chinese? => {chinese}");

    string japanese = "あなたは日本語を話しますか？";
    Elog.Info($"Do you speak Japanese? => {japanese}");

    string portuguese = "Você fala português?";
    Elog.Info($"Do you speak Portuguese? => {portuguese}");

    string russian = "а ты говоришь по русски?";
    Elog.Info($"Do you speak Russian? => {russian}");
$$ language plcsharp;

do $$
    string emoji = "🐂";
    Elog.Info($"The emoji \"{emoji}\" has lenght {emoji.Length}.");

    emoji = "\ud83e\udd70";
    Elog.Info($"The emoji \"{emoji}\" has lenght {emoji.Length}.");
$$ language plcsharp;
