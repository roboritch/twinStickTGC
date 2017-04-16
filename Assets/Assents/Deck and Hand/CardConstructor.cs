using System;

public static class CardConstructor {
	


	/// <summary>
	/// create a card from type
	/// </summary>
	/// <param name="cardType">must not be null</param>
	/// <returns>true if card created</returns>
	public static bool constructCard(Type cardType, out Card card) {
		if (cardType == null) {
			card = null;
			return false;
		}
		card = (Card)Activator.CreateInstance(cardType);
		return true;
	}


}
