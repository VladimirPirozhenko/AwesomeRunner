using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManagerTech : MonoBehaviour
{
	[Header("What Menu Is Active?")]
	public bool simpleMenu = false;
	public bool advancedMenu = true;

	[Header("Simple Panels")]
	[Tooltip("The UI Panel holding the Home Screen elements")]
	public GameObject homeScreen;
	[Tooltip("The UI Panel holding the credits")]
	public GameObject creditsScreen;
	[Tooltip("The UI Panel holding the settings")]
	public GameObject systemScreen;
	[Tooltip("The UI Panel holding the CANCEL or ACCEPT Options for New Game")]
	public GameObject newGameScreen;
	[Tooltip("The UI Panel holding the YES or NO Options for Load Game")]
	public GameObject loadGameScreen;
	[Tooltip("The Loading Screen holding loading bar")]
	public GameObject loadingScreen;

	[Header("COLORS - Tint")]
	public Image[] panelGraphics;
	public Image[] blurs;
	public Color tint;

	[Header("ADVANDED - Panels")]
	[Tooltip("The UI Panel holding the New Account Screen elements")]
	public GameObject newAccountScreen;
	[Tooltip("The UI Panel holding the Delete Account Screen elements")]
	public GameObject deleteAccountScreen;
	[Tooltip("The UI Panel holding Log-In Buttons")]
	public GameObject loginScreen;
	[Tooltip("The UI Panel holding account and load menu")]
	public GameObject databaseScreen;
	[Tooltip("The UI Menu Bar at the edge of the screen")]
	public GameObject menuBar;

	[Header("ADVANDED - UI Elements & User Data")]
	[Tooltip("The Main Canvas Gameobject")]
	public CanvasScaler mainCanvas;
	[Tooltip("The dropdown menu containing all the resolutions that your game can adapt to")]
	public TMP_Dropdown ResolutionDropDown;
	private Resolution[] resolutions;
	[Tooltip("The text object in the Settings Panel displaying the current quality setting enabled")]
	public TMP_Text qualityText; // text displaying current selected quality
	[Tooltip("The icon showing the current quality selected in the Settings Panels")]
	public Animator qualityDisplay;
	private string[] qualityNames;
	private int tempQualityLevel;// store it for start up text update
	[Tooltip("The volume slider UI element in the Settings Screen")]
	public Slider audioSlider;

	[Tooltip("If a message is displaying indiciating FAILURE, this is the color of that error text")]
	public Color errorColor;
	[Tooltip("If a message is displaying indiciating SUCCESS, this is the color of that success text")]
	public Color successColor;
	public float messageDisplayLength = 2.0f;
	public Slider uiScaleSlider;
	float xScale = 0f;
	float yScale = 0f;

	[Header("Menu Bar")]
	public bool showMenuBar = true;
	[Tooltip("The Arrow at the corner of the screen activating and de-activating the menu bar")]
	public GameObject menuBarButton;
	[Tooltip("The date and time display text at the bottom of the screen")]
	public TMP_Text dateDisplay;
	public TMP_Text timeDisplay;
	public bool showDate = true;
	public bool showTime = true;

	[Header("Loading Screen Elements")]
	[Tooltip("The name of the scene loaded when a 'NEW GAME' is started")]
	public string newSceneName;
	[Tooltip("The loading bar Slider UI element in the Loading Screen")]
	public Slider loadingBar;
	private string loadSceneName; // scene name is defined when the load game data is retrieved

	[Header("Register Account")]
	public TMP_InputField username;
	public TMP_InputField password;
	public TMP_InputField confPassword;
	public TMP_Text error_NewAccount;
	public TMP_Text messageDisplayDatabase;
	public string newAccountMessageDisplay = "ACCOUNT CREATED";
	private string Username;
	private string Password;
	private string ConfPassword;
	private string form;
	string m_Path;
	private string[] Characters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
								   "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
								   "1","2","3","4","5","6","7","8","9","0","_","-"};

	[Header("Login Account")]
	public TMP_InputField logUsername;
	public TMP_InputField logPassword;
	private string logUsernameString; // the input in logUsername
	private string logPasswordString; // the input in logPassword
	private String[] Lines;
	private string DecryptedPass;
	public TMP_Text error_LogIn;
	public TMP_Text profileDisplay;
	public string loginMessageDisplay = "LOGGED IN";

	[Header("Delete Account")]
	public TMP_InputField delUsername;
	public TMP_InputField delPassword;
	private string delUsernameString; // the input in delUsername
	private string delPasswordString; // the input in delPassword
	private String[] delLines;
	private string delDecryptedPass;
	public TMP_Text error_Delete;
	public string deletedMessageDisplay = "ACCOUNT DELETED";

	[Header("Settings Screen")]
	public TMP_Text textSpeakers;
	public TMP_Text textSubtitleLanguage;
	public List<string> speakers = new List<string>();
	public List<string> subtitleLanguage = new List<string>();

	[Header("Starting Options Values")]
	public int speakersDefault = 0;
	public int subtitleLanguageDefault = 0;

	[Header("List Indexing")]
	int speakersIndex = 0;
	int subtitleLanguageIndex = 0;

	[Header("Debug")]
	[Tooltip("If this is true, pressing 'R' will reload the scene.")]
	public bool reloadSceneButton = true;
	Transform tempParent;

	public void MoveToFront(GameObject currentObj){
		//tempParent = currentObj.transform.parent;
		tempParent = currentObj.transform;
		tempParent.SetAsLastSibling();
	}

	void Start(){
		// By default, starts on the home screen, disables others
		homeScreen.SetActive(true);
		if(newAccountScreen != null)
		newAccountScreen.SetActive(false);
		if(deleteAccountScreen != null)
		deleteAccountScreen.SetActive(false);
		if(loginScreen != null)
		loginScreen.SetActive(false);
		if(databaseScreen != null)
		databaseScreen.SetActive(false);
		if(creditsScreen != null)
		creditsScreen.SetActive(false);
		if(systemScreen != null)
		systemScreen.SetActive(false);
		if(loadingScreen != null)
		loadingScreen.SetActive(false);
		if(loadGameScreen != null)
		loadGameScreen.SetActive(false);
		if(newGameScreen != null)
		newGameScreen.SetActive(false);

		if(advancedMenu){
			// Set Save Path to local
			m_Path = Application.dataPath;
			UpdateAccountValues();
		}

		if(menuBar != null){
			if(!showMenuBar){
				menuBar.gameObject.SetActive(false);
				menuBarButton.gameObject.SetActive(false);
			}
		}

		// Set Colors if the user didn't before play
		for(int i = 0; i < panelGraphics.Length; i++)
        {
           panelGraphics[i].color = tint;
        }
		for(int i = 0; i < blurs.Length; i++)
        {
           blurs[i].material.SetColor("_Color",tint);
        }

		// Get quality settings names
		qualityNames = QualitySettings.names;

		// Get screens possible resolutions
		resolutions = Screen.resolutions;

		// Set Drop Down resolution options according to possible screen resolutions of your monitor
		if(ResolutionDropDown != null){
		for (int i = 0; i < resolutions.Length; i++){
				ResolutionDropDown.options.Add (new TMP_Dropdown.OptionData (ResToString (resolutions [i])));
	
				ResolutionDropDown.value = i;
	
				ResolutionDropDown.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions
				[ResolutionDropDown.value].width, resolutions[ResolutionDropDown.value].height, true);});
			}
		}
		 
		 // Check if first time so the volume can be set to MAX
		 if(PlayerPrefs.GetInt("firsttime")==0){
			 // it's the player's first time. Set to false now...
			 PlayerPrefs.SetInt("firsttime",1);
			 PlayerPrefs.SetFloat("volume",1);
		 }

		 // Check volume that was saved from last play
		 if(audioSlider != null)
		 audioSlider.value = PlayerPrefs.GetFloat("volume");

		// Settings screen
		speakersIndex = speakersDefault;
		subtitleLanguageIndex = subtitleLanguageDefault;

		textSpeakers.text = speakers[speakersDefault];
		textSubtitleLanguage.text = subtitleLanguage[subtitleLanguageDefault];
	}

	public void IncreaseIndex(int i){
		switch (i){
			case 0:
				if(speakersIndex != speakers.Count -1){speakersIndex++;}else{speakersIndex = 0;}
				textSpeakers.text = speakers[speakersIndex];
				break;
			case 1:
				if(subtitleLanguageIndex != subtitleLanguage.Count -1){subtitleLanguageIndex++;}else{subtitleLanguageIndex = 0;}
				textSubtitleLanguage.text = subtitleLanguage[subtitleLanguageIndex];
				break;
			default:
				break;
		}
	}

	public void DecreaseIndex(int i){
		switch (i){
			case 0:
				if(speakersIndex == 0){speakersIndex = speakers.Count;}speakersIndex--;
				textSpeakers.text = speakers[speakersIndex];
				break;
			case 1:
				if(subtitleLanguageIndex == 0){subtitleLanguageIndex = subtitleLanguage.Count;}subtitleLanguageIndex--;
				textSubtitleLanguage.text = subtitleLanguage[subtitleLanguageIndex];
				break;
			default:
				break;
		}
	}

	public void SetTint(){
		for(int i = 0; i < panelGraphics.Length; i++)
        {
           panelGraphics[i].color = tint;
        }
		for(int i = 0; i < blurs.Length; i++)
        {
           blurs[i].material.SetColor("_Color",tint);
        }
	}

	// Just for reloading the scene! You can delete this function entirely if you want to
	void Update(){
		if(reloadSceneButton){
			if(Input.GetKeyDown(KeyCode.Delete)){
				SceneManager.LoadScene("Tech Demo Scene");
			}
		}

		SetTint();

		if(showMenuBar){
			// Menu Bar and Clock/Date Elements
			DateTime time = DateTime.Now;
			if(showTime){timeDisplay.text = time.Hour + ":" + time.Minute + ":" + time.Second;}else if(!showTime){timeDisplay.text = "";}
			if(showDate){dateDisplay.text = System.DateTime.Now.ToString("yyyy/MM/dd");}else if(!showDate){dateDisplay.text = "";}
		}
	}

	// called when returned back to the database menu, confirmation message displays temporarily
	public void MessageDisplayDatabase(string message, Color col){
		StartCoroutine(MessageDisplay(message, col));
	}

	IEnumerator MessageDisplay(string message, Color col){ // Display and then clear
		messageDisplayDatabase.color = col;
		messageDisplayDatabase.text = message;
		yield return new WaitForSeconds(messageDisplayLength);
		messageDisplayDatabase.text = "";
	}

	public void UIScaler(){
		xScale = 1920 * uiScaleSlider.value;
		yScale = 1080 * uiScaleSlider.value;
		mainCanvas.referenceResolution = new Vector2 (xScale,yScale);
	}

	// Make sure all the settings panel text are displaying current quality settings properly and updating UI
	public void CheckSettings(){
		tempQualityLevel = QualitySettings.GetQualityLevel(); 
		if(tempQualityLevel == 0){
			qualityText.text = qualityNames[0];
			qualityDisplay.SetTrigger("Low");
		}else if(tempQualityLevel == 1){
			qualityText.text = qualityNames[1];
			qualityDisplay.SetTrigger("Medium");
		}else if(tempQualityLevel == 2){
			qualityText.text = qualityNames[2];
			qualityDisplay.SetTrigger("High");
		}else if(tempQualityLevel == 3){
			qualityText.text = qualityNames[3];
			qualityDisplay.SetTrigger("Ultra");
		}
	}

	// Converts the resolution into a string form that is then used in the dropdown list as the options
	string ResToString(Resolution res)
	{
		return res.width + " x " + res.height;
	}

	// Whenever a value on the audio slider in the settings panel is changed, this 
	// function is called and updated the overall game volume
	public void AudioSlider(){
		AudioListener.volume = audioSlider.value;
		PlayerPrefs.SetFloat("volume",audioSlider.value);
	}

	// When accepting the QUIT question, the application will close 
	// (Only works in Executable. Disabled in Editor)
	public void Quit(){
		Application.Quit();
	} 

	// Changes the current quality settings by taking the number passed in from the UI 
	// element and matching it to the index of the Quality Settings
	public void QualityChange(int x){
		if(x == 0){
			QualitySettings.SetQualityLevel(x, true);	
			qualityText.text = qualityNames[0];
		}else if(x == 1){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[1];
		}else if(x == 2){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[2];
		}if(x == 3){
			QualitySettings.SetQualityLevel(x, true);
			qualityText.text = qualityNames[3];
		}
	}

	// Called when loading new game scene
	public void LoadNewLevel (){
		if(newSceneName != ""){
			StartCoroutine(LoadAsynchronously(newSceneName));
		}
	}

	// Called when loading saved scene
	// Add the save code in this function!
	public void LoadSavedLevel (){
		if(loadSceneName != ""){
			StartCoroutine(LoadAsynchronously(newSceneName)); // temporarily uses New Scene Name. Change this to 'loadSceneName' when you program the save data
		}
	}

	// Load Bar synching animation
	IEnumerator LoadAsynchronously (string sceneName){ // scene name is just the name of the current scene being loaded
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

		while (!operation.isDone){
			float progress = Mathf.Clamp01(operation.progress / .9f);
			
			loadingBar.value = progress;

			yield return null;
		}
	}

	public void UpdateAccountValues(){
		// Register
		Username = username.text;
		Password = password.text;
		ConfPassword = confPassword.text;

		// Log In
		logUsernameString = logUsername.text;
		logPasswordString = logPassword.text;	

		// Delete
		delUsernameString = delUsername.text;
		delPasswordString = delPassword.text;	
	}

	public void ConfirmNewAccount(){
		bool UN = false;
		bool PW = false;
		bool CPW = false;

		if (Username != ""){
			if (!System.IO.File.Exists(m_Path+"_"+Username+".txt")){
				UN = true;
			} else {
				error_NewAccount.color = errorColor;
				error_NewAccount.text = "USERNAME ALREADY TAKEN";
			}
		} else {
			error_NewAccount.color = errorColor;
			error_NewAccount.text = "INVALID USERNAME";
		}
		if (Password != ""){
			if(Password.Length > 5){
				PW = true;
			} else {
				error_NewAccount.color = errorColor;
				error_NewAccount.text = "PASSWORD IS TOO SHORT";
			}
		} else {
			error_NewAccount.color = errorColor;
			error_NewAccount.text = "INVALID PASSWORD";
		}
		if (ConfPassword != ""){
			if (ConfPassword == Password){
				CPW = true;
			} else {
				error_NewAccount.color = errorColor;
				error_NewAccount.text = "PASSWORDS MUST MATCH";
			}
		} else {
			error_NewAccount.color = errorColor;
			error_NewAccount.text = "INVALID PASSWORD";
		}
		if (UN == true&&PW == true&&CPW == true){
			bool Clear = true;
			int i = 1;
			foreach(char c in Password){
				if (Clear){
					Password = "";
					Clear = false;
				}
				i++;
				char Encrypted = (char)(c * i);
				Password += Encrypted.ToString();
			}
			form = (Username+Environment.NewLine+Environment.NewLine+Password);
			System.IO.File.WriteAllText(m_Path+"_"+Username+".txt", form);
			Username = "";
			Password = "";
			username.text = "";
			password.text = "";
			confPassword.text = "";
			error_NewAccount.text = "";
			DecryptedPass = "";

			MessageDisplayDatabase(newAccountMessageDisplay, successColor);
			print ("Registration Complete");

			databaseScreen.SetActive(true);
			newAccountScreen.SetActive(false);
		}
	}

	public void LoginButton(){
		bool UN = false;
		bool PW = false;
		if (logUsernameString != ""){
			if(System.IO.File.Exists(m_Path+"_"+logUsernameString+".txt")){
				UN = true;
				Lines = System.IO.File.ReadAllLines(m_Path+"_"+logUsernameString+".txt");
			} else {
				error_LogIn.color = errorColor;
				error_LogIn.text = "INVALID USERNAME";
			}
		} else {
			error_LogIn.color = errorColor;
				error_LogIn.text = "PLEASE ENTER USERNAME";
		}
		if (logPasswordString != ""){
			if (System.IO.File.Exists(m_Path+"_"+logUsernameString+".txt")){
				int i = 1;
				foreach(char c in Lines[2]){
					i++;
					char Decrypted = (char)(c / i);
					DecryptedPass += Decrypted.ToString();
				}
				if (logPasswordString == DecryptedPass){
					PW = true;
				} else {
					error_LogIn.color = errorColor;
					error_LogIn.text = "PASSWORD INCORRECT";
				}
			} else {
				error_LogIn.color = errorColor;
				error_LogIn.text = "PASSWORD INCORRECT";
			}
		} else {
			error_LogIn.color = errorColor;
			error_LogIn.text = "PLEASE ENTER PASSWORD";
		}
		if (UN == true&&PW == true){
			profileDisplay.text = logUsernameString;
			logUsernameString = "";
			logPasswordString = "";
			logUsername.text = "";
			logPassword.text = "";	
			error_LogIn.text = "";
			DecryptedPass = "";

			MessageDisplayDatabase(loginMessageDisplay, successColor);
			print ("Login Successful");

			databaseScreen.SetActive(true);
			loginScreen.SetActive(false);
		}
	}

	public void ConfirmDeleteAccount(){
		bool UN = false;
		bool PW = false;
		if (delUsernameString != "" && profileDisplay.text != delUsernameString){
			if(System.IO.File.Exists(m_Path+"_"+delUsernameString+".txt")){
				UN = true;
				Lines = System.IO.File.ReadAllLines(m_Path+"_"+delUsernameString+".txt");
			} else {
				error_Delete.color = errorColor;
				error_Delete.text = "INVALID USERNAME";
			}
		} else {
			error_Delete.color = errorColor;
				error_Delete.text = "ENTER VALID USERNAME";
		}
		if (delPasswordString != ""){
			if (System.IO.File.Exists(m_Path+"_"+delUsernameString+".txt")){
				int i = 1;
				foreach(char c in Lines[2]){
					i++;
					char Decrypted = (char)(c / i);
					DecryptedPass += Decrypted.ToString();
				}
				if (delPasswordString == DecryptedPass){
					PW = true;
				} else {
					error_Delete.color = errorColor;
					error_Delete.text = "PASSWORD INCORRECT";
				}
			} else {
				error_Delete.color = errorColor;
				error_Delete.text = "PASSWORD INCORRECT";
			}
		} else {
			error_Delete.color = errorColor;
			error_Delete.text = "PLEASE ENTER PASSWORD";
		}
		if (UN == true&&PW == true){
			System.IO.File.Delete(m_Path+"_"+delUsernameString+".txt");
			delUsernameString = "";
			delPasswordString = "";
			delUsername.text = "";
			delPassword.text = "";	
			error_Delete.text = "";
			DecryptedPass = "";

			MessageDisplayDatabase(deletedMessageDisplay, successColor);
			print ("Deletion Successful");

			deleteAccountScreen.SetActive(false);
			databaseScreen.SetActive(true);
		}
	}
}
