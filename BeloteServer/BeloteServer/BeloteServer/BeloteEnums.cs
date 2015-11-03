using System;

namespace BeloteServer
{
    // Статус игрового стола
    public enum TableStatus
    {
        // Стол в режиме создания - еще не записан в БД
        CREATING = 1,
        // Стол в режиме ожидания игроков
        WAITING = 2,
        // Стол в режиме игры
        PLAYING = 3,
        // Игра на столе завершена
        ENDING = 4,
        // На столе произошла ошибка
        ERROR = 5
    }

    // Тип следующей возможной ставки
    public enum BetType
    {
        // Игрок может сделать любую ставку кроме контры и реконтры - начальная ставка
        T_BET = 1,
        // Игрок может сделать любую ставку (после уже сделанной ставки), а также может ответить контрой
        T_BETABET = 2,
        // Предыдущая ставка - капут, игрок может ответить только капутом или контрой
        BET_CAPOT = 3,
        // На контру игрок может ответить только реконтрой
        BET_SURCOINCHE = 4
    }

    // Тип игровой карты: 7, 8...
    public enum CardType
    {
        // Тип какрты не определен - используется в случае ошибки
        C_UNDEFINED = 100,
        // Номера картам присвоены в порядке их следования для сортировки "без козыря"
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
        BONUS_4X = 4,
        BONUS_TERZ = 1,
        BONUS_50 = 2,
        BONUS_100 = 3
    }
}