package cn.fawdlstty.smlite.internface;

public interface IFunc_ta<TState extends Enum, TTrigger extends Enum> {
    public TState call (TTrigger trigger, Object[] args) throws Exception;
}
