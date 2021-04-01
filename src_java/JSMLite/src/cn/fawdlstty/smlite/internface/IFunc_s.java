package cn.fawdlstty.smlite.internface;

public interface IFunc_s<TState extends Enum> {
    public TState call (TState state) throws Exception;
}
