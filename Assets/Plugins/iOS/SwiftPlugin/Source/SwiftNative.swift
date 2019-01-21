//
//  SwiftNative.swift
//  SwiftPlugin
//
//  Created by WEN WEI LIN on 2019/1/21.
//  Copyright © 2019 WEN WEI LIN. All rights reserved.
//

import Foundation
import UIKit

@objc public class SwiftNative:NSObject{
    
    @objc static public let shared = SwiftNative()
    
    @objc func toRecordPage(){
        let customPage = createRecordPage()
        UIApplication.shared.keyWindow?.rootViewController?.present(customPage, animated: true, completion: nil)
    }
    
    private func createRecordPage()->UIViewController{
        let sb = UIStoryboard(name: "Record", bundle: nil)
        return sb.instantiateViewController(withIdentifier: "Record")
    }

}
