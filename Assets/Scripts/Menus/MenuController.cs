using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject menuContainer;

    public List<Menu> menus;
    public Menu currentMenu;
    public InputHandler inputHandler;

    private List<MenuItem> menuItems;
    public int highlightedMenuItemIndex = -1;

    public bool isPaused;
    public bool isMainMenu;

    private void Awake() {

        if (isMainMenu) {
            isPaused = true;
            Pause();

            if (Application.isEditor) {
                PlayerPrefs.SetInt("LastCompletedLevel", 0);
            }
        }
    }

    public void TogglePause() {

        if (isPaused && currentMenu != menus[0]) {
            ChangeMenu(menus[0]);
            return;
        }

        if (isMainMenu) return;

        isPaused = !isPaused;

        if (isPaused) {
            Pause();
        } else UnPause();
    }

    public void Pause() { //Gives room for juice when you toggle pause
        menuContainer.SetActive(true);
        ChangeMenu(menus[0]);
        inputHandler.BlockPlayerInput();
    }

    public void UnPause() {
        isPaused = false;

        ResetHighlight();

        menuItems = null;
        currentMenu = null;
        menuContainer.SetActive(false);
        inputHandler.UnBlockPlayerInput();
    }

    public void KeySelect(Vector2Int direction) {

        if (direction == Vector2Int.up || direction == Vector2Int.down) {


            if (highlightedMenuItemIndex < 0) {
                highlightedMenuItemIndex = 0;

            } else {
                menuItems[highlightedMenuItemIndex].Highlight(false);

                if (direction == Vector2Int.up) {

                    if (highlightedMenuItemIndex == 0) {
                        highlightedMenuItemIndex = menuItems.Count - 1;
                    } else highlightedMenuItemIndex--;

                } else if (direction == Vector2Int.down) {

                    if (highlightedMenuItemIndex == menuItems.Count - 1) {
                        highlightedMenuItemIndex = 0;
                    } else highlightedMenuItemIndex++;
                }
            }

            menuItems[highlightedMenuItemIndex].Highlight(true);

        } else if (highlightedMenuItemIndex > -1 &&
            menuItems[highlightedMenuItemIndex].TryGetComponent(out LRCompatibleMenuItem LRMenuItem)) {
            LRMenuItem.SendLRInput(direction);
        }
    }

    public void ChangeMenu(Menu selectedMenu) {

        ResetHighlight();

        for (int i = 0; i < menus.Count; i++) {
            Menu menu = menus[i];

            if (menu != selectedMenu) {
                menu.gameObject.SetActive(false);
            } else {
                currentMenu = menu;
                menu.gameObject.SetActive(true);
                menuItems = menu.menuItems;
            }
        }
    }

    public void SelectMenuItem() {
        menuItems[highlightedMenuItemIndex].SelectItem();
    }

    private void ResetHighlight() {
        if (highlightedMenuItemIndex >= 0) {
            menuItems[highlightedMenuItemIndex].Highlight(false);
            highlightedMenuItemIndex = -1;
        }
    }
}
