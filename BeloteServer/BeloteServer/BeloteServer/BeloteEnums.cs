using System;

namespace BeloteServer
{
    // Статус игрового стола
    public enum TableStatus
    {
        CREATING = 1,
        WAITING = 2,
        PLAYING = 3,
        ENDING = 4,
        ERROR = 5
    }

    // Тип следующей возможной ставки
    public enum BetType
    {
        T_BET = 1,
        T_BETABET = 2,
        BET_CAPOT = 3,
        BET_SURCOINCHE = 4
    }

    // Тип игровой карты: 7, 8...
    public enum CardType
    {
        C_UNDEFINED = 100,
        C_A = 7,
        C_J = 4,
        C_Q = 5,
        C_K = 6,
        C_7 = 0,
        C_8 = 1,
        C_9 = 2,
        C_10 = 3
    }

    // Масть игровой карты
    public enum CardSuit
    {
        C_NONE = 0,
        C_HEARTS = 2,
        C_CLUBS = 1,
        C_SPADES = 3,
        С_DIAMONDS = 4
    }

    // Тип заявки
    public enum OrderType
    {
        ORDER_PASS = 0,
        ORDER_BET = 1,
        ORDER_CAPOT = 2,
        ORDER_COINCHE = 3,
        ORDER_SURCOINCHE = 4
    }

    // Тип - игровая команда
    public enum BeloteTeam
    {
        TEAM_NONE = 0,
        TEAM1_1_3 = 1,
        TEAM2_2_4 = 2
    }

    // Тип - статус раздачи: торговля, игра, завершена
    public enum DistributionStatus
    {
        D_BAZAR = 1,
        D_GAME = 2,
        D_ENDED = 3
    }

    // Тип бонуса
    public enum BonusType
    {
        BONUS_NONE = 0,
        BONUS_4X = 1,
        BONUS_TERZ = 2,
        BONUS_50 = 3,
        BONUS_100 = 4
    }
}