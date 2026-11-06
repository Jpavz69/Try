#nullable enable
using Godot;
using System;

namespace Mythikmon;

public partial class BattleUI : CanvasLayer
{
	[Signal] public delegate void AttackPressedEventHandler();
	[Signal] public delegate void TalismanPressedEventHandler();
	[Signal] public delegate void RunPressedEventHandler();

	private Label enemyName = null!, playerName = null!, playerHpText = null!;
	private ProgressBar enemyHp = null!, playerHp = null!;
	private TextureRect enemySprite = null!, playerSprite = null!;
	private RichTextLabel message = null!;
	private Button talismanButton = null!;

	public override void _Ready()
	{
		enemyName = GetNode<Label>("%EnemyName");
		enemyHp = GetNode<ProgressBar>("%EnemyHp");
		enemySprite = GetNode<TextureRect>("%EnemySprite");
		playerName = GetNode<Label>("%PlayerName");
		playerHp = GetNode<ProgressBar>("%PlayerHp");
		playerHpText = GetNode<Label>("%PlayerHpText");
		playerSprite = GetNode<TextureRect>("%PlayerSprite");
		message = GetNode<RichTextLabel>("%Message");
		talismanButton = GetNode<Button>("%TalismanButton");

		GetNode<Button>("%AttackButton").Pressed += () => EmitSignal(SignalName.AttackPressed);
		talismanButton.Pressed += () => EmitSignal(SignalName.TalismanPressed);
		GetNode<Button>("%RunButton").Pressed += () => EmitSignal(SignalName.RunPressed);
	}
	
	public void SetActive(bool active)
	{
		Visible = active;
		GetNode<Control>("Root").Visible = active;
	}

	public void Refresh(Creature? enemy, Texture2D? enemyTex, Creature? mine, Texture2D? myTex,
						string text, int talismans, bool animate)
	{
		if (enemy != null)
		{
			enemyName.Text = $"{enemy.Name}  Lv.{enemy.Level}";
			enemySprite.Texture = GD.Load<Texture2D>(enemy.SpritePath);
			SetBar(enemyHp, enemy.HP, enemy.MaxHP, animate);
		}
		enemySprite.Visible = enemy != null;

		if (mine != null)
		{
			playerName.Text = $"{mine.Name}  Lv.{mine.Level}";
			playerSprite.Texture = GD.Load<Texture2D>(mine.SpritePath);
			playerHpText.Text = $"HP: {mine.HP} / {mine.MaxHP}";
			SetBar(playerHp, mine.HP, mine.MaxHP, animate);
		}

		message.Text = text;
		talismanButton.Text = $"2 Talisman ({talismans})";
	}

	private void SetBar(ProgressBar bar, int hp, int max, bool animate)
	{
		bar.MaxValue = max;
		if (animate) CreateTween().TweenProperty(bar, "value", hp, 0.4);
		else bar.Value = hp;
	}
}
