using Godot;
using System;

public partial class HealthDisplay : Sprite2D {
	public enum HealthAmount {
		Zero,
		One,
		Two,
		Three,
		Four
	}

	[Export]
	private Texture2D _healthFour;

	[Export]
	private Texture2D _healthThree;

	[Export]
	private Texture2D _healthTwo;

	[Export]
	private Texture2D _healthOne;

	[Export]
	private Texture2D _healthZero;


	public void SetHealth(HealthAmount health) {
		Texture = health switch {
			HealthAmount.Four => _healthFour,
			HealthAmount.Three => _healthThree,
			HealthAmount.Two => _healthTwo,
			HealthAmount.One => _healthOne,
			HealthAmount.Zero => _healthZero,
		};
	}
}
