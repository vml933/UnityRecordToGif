//
//  MyViewController.swift
//  Test
//
//  Created by Mark on 2019/1/21.
//  Copyright © 2019 Mark. All rights reserved.
//

import UIKit
import AVFoundation

class RecordViewController: UIViewController {

    @IBOutlet var btnRecord:UIButton!
    @IBOutlet var btnReturn:UIButton!
    
    var currentDevice:AVCaptureDevice!
    var videoFileOutput:AVCaptureMovieFileOutput!
    var cameraPreviewLayer:AVCaptureVideoPreviewLayer?
    var isRecording = false
    
    let captureSession = AVCaptureSession()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        configure()
    }
    
    private func configure(){
        captureSession.sessionPreset = AVCaptureSession.Preset.high
        
        let deviceDiscoverSession = AVCaptureDevice.DiscoverySession(deviceTypes: [.builtInWideAngleCamera], mediaType: .video, position: .front)
        guard
            let device = deviceDiscoverSession.devices.first
            else{print("Fail to get the camera device");return}
        
        currentDevice = device
        guard
            let captureDeviceInput = try? AVCaptureDeviceInput(device:currentDevice)
            else {return}
        
        videoFileOutput = AVCaptureMovieFileOutput()
        
        captureSession.addInput(captureDeviceInput)
        captureSession.addOutput(videoFileOutput)
        
        cameraPreviewLayer = AVCaptureVideoPreviewLayer(session: captureSession)
        view.layer.addSublayer(cameraPreviewLayer!)
        cameraPreviewLayer?.videoGravity = .resizeAspectFill
        cameraPreviewLayer?.frame = view.layer.frame
        
        view.bringSubview(toFront: btnRecord)
        view.bringSubview(toFront: btnReturn)

        captureSession.startRunning()
    }
    
    @IBAction func doCapture(_ sender: Any) {
        if !isRecording{
            btnReturn.isHidden = true
            isRecording = true
            UIView.animate(withDuration: 0.5,
                           delay: 0,
                           options: [.repeat, .autoreverse, .allowUserInteraction],
                           animations: {self.btnRecord.transform = CGAffineTransform(scaleX: 0.5, y: 0.5)},
                           completion: nil)
            let outputPath = NSTemporaryDirectory() + "output.mov"
            let outputFileURL = URL(fileURLWithPath: outputPath)
            videoFileOutput.startRecording(to: outputFileURL, recordingDelegate: self)
        }else{
            btnReturn.isHidden = false
            isRecording = false
            UIView.animate(withDuration: 0.5,
                           delay: 1,
                           options: [],
                           animations: {self.btnRecord.transform = CGAffineTransform(scaleX: 1, y: 1)},
                           completion: nil)
            btnRecord.layer.removeAllAnimations()
            videoFileOutput.stopRecording()
        }
    }
    
    @IBAction func doReturn(){                
        self.dismiss(animated: true) {
            //UnityPostMessage("NATIVE_BRIDGE", "NativeReceiveMsg", "hihi I'm swift")
        }
    }
    
    deinit {
        print(#file," did deinit")
    }
}

extension RecordViewController:AVCaptureFileOutputRecordingDelegate{
    
    func fileOutput(_ output: AVCaptureFileOutput,
                    didFinishRecordingTo outputFileURL: URL,
                    from connections: [AVCaptureConnection],
                    error: Error?) {
        guard
            error == nil
            else {print(error ?? "");return}
        
        print("hihi record suc, path:\(outputFileURL)")
        //UnityPostMessage("NATIVE_BRIDGE", "NativeReceiveMsg", "record video complete")
        
        //high qty
        //Regift.createGIFFromSource(outputFileURL, startTime: 0,duration: 3, frameRate: 15) { (result) in
        //normal qty
        Regift.createGIFFromSource(outputFileURL, frameCount: 16, delayTime: 0.2) { (result) in
            print("gif save to \(String(describing: result))")
            
            let imageView = UIImageView(frame: self.view.bounds)
            //waiting... use other swifygif
            imageView.image = UIImage.gif(url: result!.absoluteString)
            self.view.addSubview(imageView)
            view.bringSubview(toFront: btnReturn)
            print("add image view")
            //UnityPostMessage("NATIVE_BRIDGE", "NativeReceiveMsg", "convert gif complete")
        }
    }
}

