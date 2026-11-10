using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AuraConstrucao : MonoBehaviour
{
    [Header("Aparência")]
    public Color corAura = new Color(1f, 0.85f, 0.35f, 1f);
    public float intensidade = 0.5f;
    public float espessura = 0.5f;
    public float velocidade = 1.5f;
    [Header("Material")]
    public Material materialAura;

    private SpriteRenderer original;
    private SpriteRenderer[] contornos;

    private MaterialPropertyBlock blocoPropriedades;

    void Start()
    {
        original = GetComponent<SpriteRenderer>();

        blocoPropriedades = new MaterialPropertyBlock();

        Vector2[] direcoes =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, -1).normalized
        };

        contornos = new SpriteRenderer[direcoes.Length];

        for (int i = 0; i < direcoes.Length; i++)
        {
            GameObject objeto = new GameObject("Brilho");

            objeto.transform.SetParent(transform, false);
            objeto.transform.localPosition =
                (Vector3)(direcoes[i] * espessura);

            SpriteRenderer sr = objeto.AddComponent<SpriteRenderer>();

            sr.sprite = original.sprite;
            sr.sharedMaterial = materialAura;
            sr.flipX = original.flipX;
            sr.flipY = original.flipY;
            sr.drawMode = original.drawMode;
            sr.sortingLayerID = original.sortingLayerID;
            sr.sortingOrder = original.sortingOrder - 1;

            contornos[i] = sr;
        }
    }

    void Update()
    {
        float pulso = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;
        float alpha = Mathf.Lerp(0.08f, intensidade, pulso);

        foreach (SpriteRenderer sr in contornos)
        {
            if (sr == null) continue;

            sr.sprite = original.sprite;

            Color cor = corAura;
            cor.a = alpha;

            sr.color = Color.white;

            blocoPropriedades.Clear();
            blocoPropriedades.SetColor("_Color", cor);
            sr.SetPropertyBlock(blocoPropriedades);
        }
    }
}