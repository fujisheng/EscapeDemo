using UnityEngine.Events;

public class PopUpsManager{

    public static void ShowPopUps(string title,string content,UnityAction onSureButtonClick,UnityAction onCancelButtonClick,string sureButtonTitle,string cancelButtonTitle){
        Mediator.SendMassage("openView", "popUpView");
        Mediator.SendMassage("showPopUps", new Args(title, content, onSureButtonClick, onCancelButtonClick, sureButtonTitle, cancelButtonTitle));
    }

    public static void ShowPopUps(string title,string content,UnityAction onSureButtonClick,string sureButtonTitle){
        Mediator.SendMassage("openView", "popUpView");
        Mediator.SendMassage("showPopUps", new Args(title, content, onSureButtonClick, sureButtonTitle));
    }
    
    public static void ShowPopUps(string title,string content, UnityAction onSureButtonClick,UnityAction onCancelButtonClick){
        Mediator.SendMassage("openView", "popUpView");
        Mediator.SendMassage("showPopUps", new Args(title, content, onSureButtonClick, onCancelButtonClick));
    }
    public static void ShowPopUps(string title,string content,UnityAction onSureButtonClick){
        Mediator.SendMassage("openView", "popUpView");
        Mediator.SendMassage("showPopUps", new Args(title, content, onSureButtonClick));
    }

    public static void ShowPopUps(string title,string content){
        ShowPopUps(title, content, HidePopUps);
    }

    public static void HidePopUps(){
        Mediator.SendMassage("closeView", "popUpView");
    }
}