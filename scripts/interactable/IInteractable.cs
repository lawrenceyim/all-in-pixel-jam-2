/// <summary>
/// Needs to be implemented by an Area2D
/// </summary>
public interface IInteractable {
    public void Interact(InteractionContext interactionContext);
}

public abstract record InteractionContext;

public record PlayerInteractionContext(Player Player) : InteractionContext;