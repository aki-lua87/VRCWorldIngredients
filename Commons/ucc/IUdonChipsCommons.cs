using VRC.Udon.Common.Interfaces;

// TODO: UdonSharp does not yet support inheriting from interfaces

public interface IUdonChipsCommons
{
        float GetMoney();
        void SetMoney(float f);
        void PushUdonChips();
        void AddMoney(float f);
        void SubtractMoney(float f);
        bool IsAndOver(float f);
        bool isOverMoney(float f);
        // void ShowAllPlayerUdonChips(); private
        float GetMoneyByPlayerID(int id);
}
