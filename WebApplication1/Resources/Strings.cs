﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullCowsGameBot.Resources;
public static class Strings
{
    public static string RulesText =>
         "Компьютер загадывает четырехзначное число, где цифры не повторяются. Например, \"3 6 1 8\".\r\nВаша задача - угадать это число.\r\nВы предлагаете компьютеру свою попытку, вводя четыре различные цифры (например, \"7 2 4 9\").\r\nКомпьютер сообщает вам, сколько цифр в вашей попытке совпадают с цифрами в загаданном числе и находятся на том же месте (это \"быки\") и сколько цифр совпадают, но находятся в другом месте (это \"коровы\").\r\nВы делаете следующую попытку, и так продолжаете до тех пор, пока не угадаете число.\r\nЦель игры - угадать число, загаданное компьютером, за наименьшее количество попыток. Удачи в игре!";

    public static string StartText =>
        "Я загадал число! Можешь начинать отгадывать";

    public static string FinishText =>
        "Ты выиграл! Сыграем еще?";

    public static string NotValidMessageText =>
        "Было введено не то, что ожидалось";
    public static string CommonExeptionText =>
        "Что-то пошло не так(";

}