using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Referência para o componente Slider
    [SerializeField] private Slider slider;

    // Função para configurar o valor máximo da barra
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health; // Começa com a vida cheia
    }

    // Função para atualizar o valor atual da barra
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}