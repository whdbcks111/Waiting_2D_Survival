using UnityEngine.SceneManagement;
using UnityEngine;

public class HalicopterBlock : Block, Interactive
{
    public void Interact()
    {
        if(Player.Instance.Inventory.GetAmount(Item.SosBeacon) >= 1)
        {
            SceneManager.LoadScene(2);
        }
        else {
            AudioManager.Instance.PlayOneShot("unable_alert", 0.4f);
            Player.Instance.Alert("SOS", "<color=red>SOS 신호기가 1개 필요합니다!</color>");
        }
    }

    private void Update() {
        if(Vector2.Distance(transform.position, Player.Instance.transform.position) < 3F 
            && Player.Instance.Inventory.GetAmount(Item.SosBeacon) >= 1) {
            Player.Instance.Alert("SOS_Tip", "가까이 붙어서 우클릭으로 헬리콥터 승강장에 상호작용", 0.1f);
        }
    }
}
