using System.Collections.Generic;

public static class PlayerData {
    public static HashSet<KeyCard.Color> CardsFound = [];
    public static HashSet<KeyCard.Color> DoorsUnlocked = [];
    public static HealthDisplay.HealthAmount Health = HealthDisplay.HealthAmount.Four;
}