using UnityEngine;
using UnityEngine.UI;

// リスト項目のデータクラスを定義
public class ShopItemData
{
    public int item_id;
    public string word;
    public int count;
    public string savetime;

}

// TableViewCell<T>クラスを継承する
public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{
	private int item_id;    // 単語名を表示するテキスト
    [SerializeField] private Text wordLabel;	// 単語名を表示するテキスト
	[SerializeField] private Text countLabel;	// 回数を表示するテキスト
	[SerializeField] private Text savetimeLabel;	// 日付を表示するテキスト

	// セルの内容を更新するメソッドのオーバーライド
	public override void UpdateContent(ShopItemData itemData)
	{
        wordLabel.text = itemData.word;
        countLabel.text = itemData.count.ToString();
        savetimeLabel.text = itemData.savetime;

#region アイコンのスプライトを変更するコードの追加
		// スプライトシート名とスプライト名を指定してアイコンのスプライトを変更する
		//iconImage.sprite = SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);
		//iconImage.sprite = Resources.Load<Sprite> ("face/"+itemData.iconName);


#endregion
	}
}
