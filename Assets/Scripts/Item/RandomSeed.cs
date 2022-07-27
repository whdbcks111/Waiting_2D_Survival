using UnityEngine;

public class RandomSeed : ItemInfo
{
    public GameObject[] Seed { get; private set; }

    public RandomSeed()
    {
        Name = "알수없는 씨앗";
        Description = "무엇이 자랄지 알 수 없다.";
        ID = "random_seed";
    }
    
    public override bool RightUse(Player player)
    {
        if(Seed == null || Seed.Length == 0)
        {

            Seed = new GameObject[]
            {
                Blocks.Bean.gameObject,
                Blocks.Potato.gameObject,
                Blocks.SweetPotato.gameObject,
                Blocks.Tomato.gameObject,
            };
        }

        if (WorldGenerator.Instance.IsAnyBlockHere(Player.Instance.transform.position)) return false;
        var random = Random.Range(0, Seed.Length);
        if (random >= Seed.Length) random = Seed.Length - 1;
        print(random);
        var block = Instantiate(Seed[random], Vector3Int.FloorToInt(Player.Instance.transform.position), Quaternion.identity).GetComponent<Block>();
        block.IsDefault = false;
        block.transform.parent = WorldGenerator.Instance.transform;
        AudioManager.Instance.PlayOneShot("seed_use", .4f);
        return true;
    }
}
