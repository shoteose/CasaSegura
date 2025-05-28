using UnityEngine;
using UnityEngine.UI;

public class GestaoPlayerCena : MonoBehaviour
{
    [Header("Botoes")]
    [SerializeField] private Button[] botoes;

    [Header("PaineisPessoas")]
    [SerializeField] private GameObject[] paineis;
    [SerializeField] private RawImage[] imagens;
    [SerializeField] private Button[] botoesRemover;

    private void Awake()
    {

        for (int i = 1; i < imagens.Length; i++) {

            imagens[i].gameObject.SetActive(false);

        }
    }


    void Update()
    {
        foreach (RawImage image in imagens) {
            for (int i = 1; i < imagens.Length; i++)
            {

                if (!imagens[i].gameObject.activeSelf)
                {
                    Image img = botoesRemover[i].GetComponent<Image>();
                    Color cor = img.color;
                    cor.a = 0.3f;
                    img.color = cor;

                }

            }
        }
    }

}
