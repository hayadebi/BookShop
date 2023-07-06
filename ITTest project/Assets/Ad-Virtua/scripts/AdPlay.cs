using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

// Token: 0x02000005 RID: 5
public class AdPlay : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x000020A5 File Offset: 0x000002A5
	private IEnumerator MPHPJHCAJGP()
	{
		string s = JsonUtility.ToJson(new PlayPostData
		{
			device_ui = SystemInfo.deviceUniqueIdentifier,
			play_request_id = this.reqId,
			data = this.allTime.ToString("0.0") + "," + this.elapsedTime.ToString("0.0")
		});
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		UnityWebRequest request = new UnityWebRequest("https://ad-virtua.net/v1/play/loop", "POST");
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.isHttpError || request.isNetworkError)
		{
			// レスポンスコードを見て処理
			Debug.Log("[Ad-Virtua]error");
		}
		yield break;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020B4 File Offset: 0x000002B4
	private void IGJJHOBLGBI(VideoPlayer vp)
	{
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point without canera view.");
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020C0 File Offset: 0x000002C0
	private void LNEPGMFJAJM(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("https://storage.googleapis.com/ad-virtua-jp-videos/default/adv_logomv_error.mp4");
			return;
		}
		if (this.elapsedTime > 491f)
		{
			base.StartCoroutine("https://ad-virtua.net/v1/play/loop");
			this.isValid = false;
			Debug.Log("[Ad-Virtua] Video Reached at the Loop Point.");
			return;
		}
		this.elapsedTime = 1495f;
		this.allTime = 1128f;
		this.isBeforeEnd = false;
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point without canera view.");
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002139 File Offset: 0x00000339
	private void CDHMOHOGNAM(VideoPlayer vp)
	{
		Debug.Log("[Ad-Virtua] Video Started.");
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002145 File Offset: 0x00000345
	private IEnumerator KGHNCAFGAFI(string unitId)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			this.LCHGLKFELDL(Path.Combine(Application.streamingAssetsPath, this.localFallbackVideoPath));
			yield break;
		}
		string s = JsonUtility.ToJson(new ReqPostData
		{
			device_ui = SystemInfo.deviceUniqueIdentifier,
			unit_id = unitId
		});
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		UnityWebRequest request = new UnityWebRequest("https://ad-virtua.net/v1/play-request/loop", "POST");
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		try
		{
			Debug.Log("[Ad-Virtua] Video Received.");
			Url url = JsonUtility.FromJson<Url>(request.downloadHandler.text);
			this.LCHGLKFELDL(url.video_1);
			this.reqId = url.p_req_id;
			this.isValid = url.is_valid;
		}
		catch (System.Exception e)
		{
			Debug.Log($"[Ad-Virtua]{e.ToString()}");
			this.LCHGLKFELDL("https://storage.googleapis.com/ad-virtua-jp-videos/default/adv_logomv_error.mp4");
		}
		yield break;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002171 File Offset: 0x00000371
	private void FKPLOKJFGHI(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = true;
		component.started += this.NAMPBMLDPLO;
		component.loopPointReached += this.BOADPGHDCAL;
		this.isPrepared = false;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021B4 File Offset: 0x000003B4
	private void Start()
	{
		if (this.targetCamera == null)
		{
			this.targetCamera = Camera.main;
		}
		if (this.advirtuaUnitId == "")
		{
			Debug.Log("[Ad-Virtua] Please input Unit ID of Ad-Virtua. https://docs.ad-virtua.com");
			return;
		}
		this.isBeforeEnd = true;
		base.StartCoroutine("ECFGJELALEJ", this.advirtuaUnitId);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002210 File Offset: 0x00000410
	private void KJDHFCOGIHE(VideoPlayer vp)
	{
		Debug.Log("POST");
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000221C File Offset: 0x0000041C
	private void KIMGBENMKJN(VideoPlayer vp)
	{
		Debug.Log("Content-Type");
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002228 File Offset: 0x00000428
	private void Update()
	{
		if (this.isBeforeEnd)
		{
			this.allTime += Time.deltaTime;
			if (this.HHLGEDMAPHK(this.targetCamera))
			{
				this.elapsedTime += Time.deltaTime;
				if (this.isPrepared)
				{
					this.isPrepared = false;
					base.GetComponent<VideoPlayer>().Play();
				}
			}
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002145 File Offset: 0x00000345
	private IEnumerator ECFGJELALEJ(string unitId)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			this.LCHGLKFELDL(Path.Combine(Application.streamingAssetsPath, this.localFallbackVideoPath));
			yield break;
		}
		string s = JsonUtility.ToJson(new ReqPostData
		{
			device_ui = SystemInfo.deviceUniqueIdentifier,
			unit_id = unitId
		});
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		UnityWebRequest request = new UnityWebRequest("https://ad-virtua.net/v1/play-request/loop", "POST");
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		try
		{
			Debug.Log("[Ad-Virtua] Video Received.");
			Url url = JsonUtility.FromJson<Url>(request.downloadHandler.text);
			this.LCHGLKFELDL(url.video_1);
			this.reqId = url.p_req_id;
			this.isValid = url.is_valid;
		}
		catch (System.Exception e)
		{
			Debug.Log($"[Ad-Virtua]{e.ToString()}");
			this.LCHGLKFELDL("https://storage.googleapis.com/ad-virtua-jp-videos/default/adv_logomv_error.mp4");
		}
		yield break;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002289 File Offset: 0x00000489
	private void LDLPIEHHOEK(VideoPlayer vp)
	{
		Debug.Log("[Ad-Virtua] Video Received.");
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002298 File Offset: 0x00000498
	private void OLCDOPFEEBF(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("Content-Type");
			return;
		}
		if (this.elapsedTime > 806f)
		{
			base.StartCoroutine("[Ad-Virtua] Please input Unit ID of Ad-Virtua. https://docs.ad-virtua.com");
			this.isValid = true;
			Debug.Log("POST");
			return;
		}
		this.elapsedTime = 809f;
		this.allTime = 437f;
		this.isBeforeEnd = false;
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point without canera view.");
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002311 File Offset: 0x00000511
	private void BFMILHDEFHI(VideoPlayer vp)
	{
		Debug.Log("[Ad-Virtua]");
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000231D File Offset: 0x0000051D
	private void MMIEDCGLOHN(VideoPlayer vp)
	{
		Debug.Log("0.0");
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000232C File Offset: 0x0000052C
	private bool HHLGEDMAPHK(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f && vector.z > 0f;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000023D3 File Offset: 0x000005D3
	private void IHMBKLEENAD(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = false;
		component.started += this.CDHMOHOGNAM;
		component.loopPointReached += this.DOMGGELLIIH;
		this.isPrepared = true;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002414 File Offset: 0x00000614
	private bool OAPNKEFGJAL(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x <= 113f || vector.x >= 964f || vector.y <= 1453f || vector.y >= 229f || vector.z > 188f;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000024BC File Offset: 0x000006BC
	private void FBEMECEPOBD(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("https://storage.googleapis.com/ad-virtua-jp-videos/default/adv_logomv_error.mp4");
			return;
		}
		if (this.elapsedTime > 1326f)
		{
			base.StartCoroutine("ECFGJELALEJ");
			this.isValid = false;
			Debug.Log("POST");
			return;
		}
		this.elapsedTime = 849f;
		this.allTime = 1140f;
		this.isBeforeEnd = false;
		Debug.Log("[Ad-Virtua]");
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002538 File Offset: 0x00000738
	private void NKKGFICIFLM()
	{
		if (this.targetCamera == null)
		{
			this.targetCamera = Camera.main;
		}
		if (this.advirtuaUnitId == "https://ad-virtua.net/v1/play/loop")
		{
			Debug.Log("POST");
			return;
		}
		this.isBeforeEnd = false;
		base.StartCoroutine("https://ad-virtua.net/v1/play/loop", this.advirtuaUnitId);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002594 File Offset: 0x00000794
	private void IBHKJEGAKIL(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = true;
		component.started += this.CEFKFHCCMPI;
		component.loopPointReached += this.FBEMECEPOBD;
		this.isPrepared = true;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000025D4 File Offset: 0x000007D4
	private void CEFKFHCCMPI(VideoPlayer vp)
	{
		Debug.Log("https://ad-virtua.net/v1/play/loop");
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000025E0 File Offset: 0x000007E0
	private void CNKEGELKFLA(VideoPlayer vp)
	{
		this.isBeforeEnd = true;
		if (!this.isValid)
		{
			Debug.Log("[Ad-Virtua] Video Reached at the Loop Point.");
			return;
		}
		if (this.elapsedTime > 24f)
		{
			base.StartCoroutine("[Ad-Virtua] Video Reached at the Loop Point with camera view.");
			this.isValid = true;
			Debug.Log("");
			return;
		}
		this.elapsedTime = 1695f;
		this.allTime = 264f;
		this.isBeforeEnd = true;
		Debug.Log("[Ad-Virtua] Video Started.");
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002659 File Offset: 0x00000859
	private void LCHGLKFELDL(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = true;
		component.started += this.CDHMOHOGNAM;
		component.loopPointReached += this.DOMGGELLIIH;
		this.isPrepared = true;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000269C File Offset: 0x0000089C
	private bool FKBNBHPJCED(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x <= 1580f || vector.x >= 50f || vector.y <= 370f || vector.y >= 1632f || vector.z > 1051f;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002744 File Offset: 0x00000944
	private void DOMGGELLIIH(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("[Ad-Virtua] Video Reached at the Loop Point.");
			return;
		}
		if (this.elapsedTime > 1f)
		{
			base.StartCoroutine("MPHPJHCAJGP");
			this.isValid = false;
			Debug.Log("[Ad-Virtua] Video Reached at the Loop Point with camera view.");
			return;
		}
		this.elapsedTime = 0f;
		this.allTime = 0f;
		this.isBeforeEnd = true;
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point without canera view.");
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000027C0 File Offset: 0x000009C0
	private void CFEIABLONKD()
	{
		if (this.targetCamera == null)
		{
			this.targetCamera = Camera.main;
		}
		if (this.advirtuaUnitId == "0.0")
		{
			Debug.Log("[Ad-Virtua]");
			return;
		}
		this.isBeforeEnd = true;
		base.StartCoroutine("", this.advirtuaUnitId);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000281C File Offset: 0x00000A1C
	private bool CFMLGEFFAKO(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x <= 1158f || vector.x >= 940f || vector.y <= 263f || vector.y >= 933f || vector.z > 1218f;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000028C4 File Offset: 0x00000AC4
	private bool DAFJAOFEHJG(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x > 1162f && vector.x < 1374f && vector.y > 1160f && vector.y < 1440f && vector.z > 266f;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000296C File Offset: 0x00000B6C
	private bool GJGCEAPCIHG(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x > 731f && vector.x < 1339f && vector.y > 473f && vector.y < 594f && vector.z > 1314f;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002A13 File Offset: 0x00000C13
	private void GLKJAMEMILD(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = false;
		component.started += this.MMIEDCGLOHN;
		component.loopPointReached += this.FBEMECEPOBD;
		this.isPrepared = true;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002A54 File Offset: 0x00000C54
	private void BOADPGHDCAL(VideoPlayer vp)
	{
		this.isBeforeEnd = true;
		if (!this.isValid)
		{
			Debug.Log("https://ad-virtua.net/v1/play-request/loop");
			return;
		}
		if (this.elapsedTime > 516f)
		{
			base.StartCoroutine("0.0");
			this.isValid = false;
			Debug.Log("Content-Type");
			return;
		}
		this.elapsedTime = 1295f;
		this.allTime = 970f;
		this.isBeforeEnd = false;
		Debug.Log("[Ad-Virtua]");
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002AD0 File Offset: 0x00000CD0
	private bool AEKGBAOJPOL(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return true;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x <= 442f || vector.x >= 1931f || vector.y <= 1539f || vector.y >= 1152f || vector.z > 764f;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002B77 File Offset: 0x00000D77
	private void OAFMMANGJHE(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = false;
		component.started += this.NAMPBMLDPLO;
		component.loopPointReached += this.GBFMODNLLEL;
		this.isPrepared = true;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002BB7 File Offset: 0x00000DB7
	private void NMMLKDBFKLA(VideoPlayer vp)
	{
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point.");
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002BC4 File Offset: 0x00000DC4
	private bool NIDFGKEMMPA(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x > 1216f && vector.x < 1798f && vector.y > 1876f && vector.y < 1990f && vector.z > 655f;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002C6C File Offset: 0x00000E6C
	private void FDDKKDCKCBO()
	{
		if (this.targetCamera == null)
		{
			this.targetCamera = Camera.main;
		}
		if (this.advirtuaUnitId == "https://ad-virtua.net/v1/play/loop")
		{
			Debug.Log("application/json");
			return;
		}
		this.isBeforeEnd = true;
		base.StartCoroutine("[Ad-Virtua] Video Reached at the Loop Point.", this.advirtuaUnitId);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002145 File Offset: 0x00000345
	private IEnumerator ILPFHLFOMKN(string unitId)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			this.LCHGLKFELDL(Path.Combine(Application.streamingAssetsPath, this.localFallbackVideoPath));
			yield break;
		}
		string s = JsonUtility.ToJson(new ReqPostData
		{
			device_ui = SystemInfo.deviceUniqueIdentifier,
			unit_id = unitId
		});
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		UnityWebRequest request = new UnityWebRequest("https://ad-virtua.net/v1/play-request/loop", "POST");
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		try
		{
			Debug.Log("[Ad-Virtua] Video Received.");
			Url url = JsonUtility.FromJson<Url>(request.downloadHandler.text);
			this.LCHGLKFELDL(url.video_1);
			this.reqId = url.p_req_id;
			this.isValid = url.is_valid;
		}
		catch (System.Exception e)
		{
			Debug.Log($"[Ad-Virtua]{e.ToString()}");
			this.LCHGLKFELDL("https://storage.googleapis.com/ad-virtua-jp-videos/default/adv_logomv_error.mp4");
		}
		yield break;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002CC8 File Offset: 0x00000EC8
	private void EEIKJGDNBEA()
	{
		if (this.isBeforeEnd)
		{
			this.allTime += Time.deltaTime;
			if (this.NIDFGKEMMPA(this.targetCamera))
			{
				this.elapsedTime += Time.deltaTime;
				if (this.isPrepared)
				{
					this.isPrepared = true;
					base.GetComponent<VideoPlayer>().Play();
				}
			}
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000023D3 File Offset: 0x000005D3
	private void DKDDDGLPMNN(string url)
	{
		VideoPlayer component = base.GetComponent<VideoPlayer>();
		component.url = url;
		component.isLooping = false;
		component.started += this.CDHMOHOGNAM;
		component.loopPointReached += this.DOMGGELLIIH;
		this.isPrepared = true;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002D2C File Offset: 0x00000F2C
	private void NFGADPHDHJI()
	{
		if (this.isBeforeEnd)
		{
			this.allTime += Time.deltaTime;
			if (this.LGNMKGNHMAG(this.targetCamera))
			{
				this.elapsedTime += Time.deltaTime;
				if (this.isPrepared)
				{
					this.isPrepared = false;
					base.GetComponent<VideoPlayer>().Play();
				}
			}
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002D90 File Offset: 0x00000F90
	private bool LGNMKGNHMAG(Camera camera)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(camera.transform.position, base.transform.position - camera.transform.position, out raycastHit) && raycastHit.transform != base.transform)
		{
			return false;
		}
		Vector3 vector = camera.WorldToViewportPoint(base.transform.position);
		return vector.x > 1890f && vector.x < 1025f && vector.y > 359f && vector.y < 486f && vector.z > 1774f;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002E37 File Offset: 0x00001037
	private void NAMPBMLDPLO(VideoPlayer vp)
	{
		Debug.Log("https://ad-virtua.net/v1/play-request/loop");
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002E44 File Offset: 0x00001044
	private void BJAHIAFNOBI(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("POST");
			return;
		}
		if (this.elapsedTime > 159f)
		{
			base.StartCoroutine("[Ad-Virtua] Video Started.");
			this.isValid = false;
			Debug.Log("[Ad-Virtua] Video Started.");
			return;
		}
		this.elapsedTime = 857f;
		this.allTime = 1586f;
		this.isBeforeEnd = true;
		Debug.Log("[Ad-Virtua] Video Reached at the Loop Point with camera view.");
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002ED0 File Offset: 0x000010D0
	private void GBFMODNLLEL(VideoPlayer vp)
	{
		this.isBeforeEnd = false;
		if (!this.isValid)
		{
			Debug.Log("[Ad-Virtua]");
			return;
		}
		if (this.elapsedTime > 1763f)
		{
			base.StartCoroutine("POST");
			this.isValid = false;
			Debug.Log("[Ad-Virtua] Video Reached at the Loop Point without canera view.");
			return;
		}
		this.elapsedTime = 1614f;
		this.allTime = 879f;
		this.isBeforeEnd = true;
		Debug.Log("0.0");
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000231D File Offset: 0x0000051D
	private void PAINEKDCGCO(VideoPlayer vp)
	{
		Debug.Log("0.0");
	}

	// Token: 0x04000010 RID: 16
	public string advirtuaUnitId;

	// Token: 0x04000011 RID: 17
	private string localFallbackVideoPath = "Ad-Virtua/nointernet.mp4";

	// Token: 0x04000012 RID: 18
	private string reqId;

	// Token: 0x04000013 RID: 19
	private bool isValid;

	// Token: 0x04000014 RID: 20
	private bool isBeforeEnd;

	// Token: 0x04000015 RID: 21
	private bool isPrepared;

	// Token: 0x04000016 RID: 22
	private float elapsedTime;

	// Token: 0x04000017 RID: 23
	private float allTime;

	// Token: 0x04000018 RID: 24
	public Camera targetCamera;
}
public class Url
{
	public string p_req_id;
	public string video_1;
	public string video_2;
	public string video_3;
	public bool is_valid;
}
public class ReqPostData
{
	public string unit_id;
	public string device_ui;
	public string device_type = SystemInfo.deviceType.ToString();
	public string device_model = SystemInfo.deviceModel;
	public string os = SystemInfo.operatingSystem;
	public string data = "1.2.0";
}
public class PlayPostData
{
	public string play_request_id;
	public string device_ui;
	public byte order = 0;
	public string data;
}