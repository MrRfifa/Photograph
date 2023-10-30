import {
  FaGithubSquare,
  FaGlobe,
  FaLinkedin,
  FaRegEnvelope,
} from "react-icons/fa";
import camLogo from "../assets/cam.png";
import { Link } from "react-router-dom";

const Footer = () => {
  return (
    <div className="max-w-[1240px] mx-auto py-16 px-4 grid lg:grid-cols-3 gap-8 text-gray-300 ">
      <div>
        <div className="w-full text-3xl font-bold text-[#00df9a]">
          <img className="w-16" src={camLogo} alt="cam" />
        </div>
        <p className="py-4">
          Lorem, ipsum dolor sit amet consectetur adipisicing elit. Id odit
          ullam iste repellat consequatur libero reiciendis, blanditiis
          accusantium.
        </p>
        <div className="flex justify-between md:w-[75%] my-6">
          <Link
            to="https://github.com/MrRfifa"
            target="_blank"
            className="transition duration-300 ease-in-out transform hover:scale-150 hover:z-10 "
          >
            <FaGithubSquare size={30} />
          </Link>
          <Link
            to="https://www.linkedin.com/in/anouar-rfifa/"
            target="_blank"
            className="transition duration-300 ease-in-out transform hover:scale-150 hover:z-10 "
          >
            <FaLinkedin size={30} />
          </Link>
          <a
            rel="noreferrer"
            href="mailto:anouarrafifa99@gmail.com"
            target="_blank"
            className="transition duration-300 ease-in-out transform hover:scale-150 hover:z-10"
          >
            <FaRegEnvelope size={30} />
          </a>

          <Link
            to="https://mr-rfifa.vercel.app"
            target="_blank"
            className="transition duration-300 ease-in-out transform hover:scale-150 hover:z-10 "
          >
            <FaGlobe size={30} />
          </Link>
        </div>
      </div>
      <div className="lg:col-span-2 flex justify-between mt-6">
        <div>
          <h6 className="font-medium text-gray-400">Support</h6>
          <ul>
            <li className="py-2 text-sm">Pricing</li>
            <li className="py-2 text-sm">Documentation</li>
            <li className="py-2 text-sm">Guides</li>
            <li className="py-2 text-sm">API Status</li>
          </ul>
        </div>
        <div>
          <h6 className="font-medium text-gray-400">Company</h6>
          <ul>
            <li className="py-2 text-sm">About</li>
            <li className="py-2 text-sm">Blog</li>
            <li className="py-2 text-sm">Jobs</li>
          </ul>
        </div>
        <div>
          <h6 className="font-medium text-gray-400">Legal</h6>
          <ul>
            <li className="py-2 text-sm">Claim</li>
            <li className="py-2 text-sm">Policy</li>
            <li className="py-2 text-sm">Terms</li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Footer;
